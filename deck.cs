
using System;
using System.IO;
using System.Collections.Generic;
using System.Text.Json;

namespace deck {

	// The serializer only wants "properties" not fields
	// Use a class here because we want this to be inherited
	// CoreDeck, CardContent, and CardMeta contain only properties
	public class CoreDeck {
		protected uint nextID { get; set; } = 1;
		public uint start_interval { get; set; } = 1; // in days
		// No GetFloat method for the json reader
		public double punish { get; set; } = 0.5;
		public double hard { get; set; } = 0.8;
		public double bonus { get; set; } = 0.3;
		public uint studyLimit { get; set; } = uint.MaxValue;
		public uint newCardspDay { get; set; } = 5;
	}

	public class Deck : CoreDeck {
		public readonly static string saveFolder = Path.Combine(
			Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
			"WinFlash");

		public static readonly string[] emptyMessage = new string[] {
			"There is no more studying to be done.",
			"Please click on the continue button to go back to the main menu"
		};

		readonly static JsonSerializerOptions jopts = new JsonSerializerOptions {
			WriteIndented = true,
			ReadCommentHandling = JsonCommentHandling.Skip,
			AllowTrailingCommas = true
		};

		CardContent[]? contents;
		List<CardMeta> metas;
		public string name { get; }
		public string dirName { get; }
		public string fnContent { get; }
		public string fnMeta { get; }
		public string fnDeck { get; }
		public Card[] cards;
		public Queue<Card> newCards;
		// Option to attach new cards to the other end?
		public Queue<Card> studyCards;

		private static void getMetaInfo(uint? id, List<CardMeta> arr,
				out CardMeta res) {
			foreach (CardMeta m in arr) {
				if (m.ID == id) {
					res = m;
					return;
				}
			}
			res = new CardMeta(id);
			arr.Add(res);
		}

		public Deck(string name) {
			this.name = name;
			dirName = Path.Combine(saveFolder, name);
			fnContent = Path.Combine(dirName, name + ".json");
			fnDeck = Path.Combine(dirName, name + "-deck.json");
			fnMeta = Path.Combine(dirName, name + "-meta.json");
			try {
				Directory.CreateDirectory(dirName);
				string strContent = File.ReadAllText(fnContent);
				contents = JsonSerializer.Deserialize<CardContent[]>(strContent,
					jopts);
				// It would be nice if I could do this:
				// base = JsonSerializer.Deserialize<CoreDeck>(strDeck, jopts);
				// BUT this allows for partial configuration which is nice
				if (File.Exists(fnDeck) && new FileInfo(fnDeck).Length != 0) {
					byte[] deckData = File.ReadAllBytes(fnDeck);
					string? prop;
					Utf8JsonReader deckReader = new Utf8JsonReader(deckData);
					while (deckReader.Read()) {
						if (deckReader.TokenType == JsonTokenType.PropertyName) {
							prop = deckReader.GetString();
							switch (prop) {
								case "nextID":
									base.nextID = deckReader.GetUInt32();
									break;
								case "start_interval":
									base.start_interval = deckReader.GetUInt32();
									break;
								case "punish":
									base.punish = deckReader.GetDouble();
									break;
								case "hard":
									base.hard = deckReader.GetDouble();
									break;
								case "bonus":
									base.bonus = deckReader.GetDouble();
									break;
								case "studyLimit":
									base.studyLimit = deckReader.GetUInt32();
									break;
								case "newCardspDay":
									base.newCardspDay = deckReader.GetUInt32();
									break;
							}
						}
					}
				}
				string strMeta = File.ReadAllText(fnMeta);
				metas = JsonSerializer.Deserialize<List<CardMeta>>(strMeta, jopts);
			} catch (FileNotFoundException ex) {
				// Content will be handled by ui. Deck should never throw
				if (ex.FileName == fnContent) { throw; }
				if (ex.FileName == fnMeta) {
					metas = new List<CardMeta>();
				}
			} catch (JsonException ex) {
				throw;
			}
			cards = new Card[contents.Count()];
			newCards = new Queue<Card>();
			studyCards = new Queue<Card>();

			// Zip the contents and meta info together to make the Card objects
			for (uint i = 0; i < contents.Count(); i++) {
				cards[i].content = contents[i];
				getMetaInfo(contents[i].ID, metas, out cards[i].meta);
				if (cards[i].meta.last_study == null) {
					if (cards[i].meta.ID == null) {
						cards[i].content.ID = nextID;
						cards[i].meta.ID = nextID;
						nextID++;
						cards[i].meta.wait = start_interval;
					}
					if (newCards.Count < newCardspDay) {
						newCards.Enqueue(cards[i]);
					}
				} else {
					cards[i].content.ID = cards[i].meta.ID;
					if (studyCards.Count() < studyLimit && DateTime.Now
						- cards[i].meta.last_study >= TimeSpan.FromDays(
							cards[i].meta.wait)
						) {
						studyCards.Enqueue(cards[i]);
					}
				}
			}
		}

		public void grade(ref Card c, int q) {
			// This if statement should be put elsewhere
			if (c.meta.wait == 0) {
				c.meta.wait = start_interval;
			}
			double fwait = c.meta.wait;
			switch (q) {
				case 1:
					fwait *= punish;
					studyCards.Enqueue(c);
					break;
				case 2:
					fwait *= hard;
					studyCards.Enqueue(c);
					break;
				case 3:
					fwait *= c.meta.EF;
					break;
				case 4:
					fwait *= c.meta.EF + bonus;
					break;
			}
			c.meta.wait = (uint)Math.Round(fwait);
			c.meta.EF += 0.1 - (4 - q)*(0.08 + (4 - q)*0.02);
			c.meta.last_study = DateTime.Now;
		}

		// Picks the next card to study for our user controls
		public Card nextCard() {
			try {
				return this.newCards.Dequeue();
			}
			catch (InvalidOperationException e)
					when (this.studyCards.Count > 0) {
				return this.studyCards.Dequeue();
			}
			catch (InvalidOperationException) {
				throw;
			}
		}

		// Only save the meta info for now.
		// This may change in the future
		public void save() {
			string strContent = JsonSerializer.Serialize(contents, jopts);
			string strMeta = JsonSerializer.Serialize(metas, jopts);
			File.WriteAllText(fnContent, strContent);
			File.WriteAllText(fnMeta, strMeta);
		}
	}

	// We split the card into two classes so that the user can create new cards
	// without dealing with study info
	public struct Card {
		public CardContent content;
		public CardMeta meta;
	}

	public enum Side { Front, Back }
	// These are both classes because we want to use them as reference types
	public class CardMeta {
		public uint? ID { get; set; }
		// We'll use this nullable type to tell if a card has been studied or not
		public DateTime? last_study { get; set; } = null;
		public uint rn { get; set; } = 0; // repetition number
		public uint wait { get; set; } = 0; // days to wait since last study
		// easiness factor; part of supermemo2
		public double EF { get; set; } = 2.5;

		public CardMeta(uint? id) {
			this.ID = id;
		}
	}

	public class CardContent {
		public uint? ID { get; set; }
		public string[] front { get; set; }
		public string[] back { get; set; }
		public string[] tags { get; set; }
	}
}
