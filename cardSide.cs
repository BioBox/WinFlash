using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using deck;

namespace WinFlash
{
	public partial class CardSide : UserControl
	{
		// Pointer to the string we want to draw
		private string[]? drawref = null;
		private const Single leftPadding = 10;
		// TODO: Make these two configurable by user
		private Font font = SystemFonts.DefaultFont;
		private Brush defaultBrush = Brushes.Black;
		private Button punishButton, hardButton, goodButton, easyButton;
		private Button continueButton;
		private Deck deck;
		private Card currentCard;
		private EventHandler flip, toMenu;
		private event EventHandler studyDone;
		// Needs a reference to the menu so we can establish is as the "owner"
		// We need this to tell the main menu to recreate itself when we're done
		// studying.
		// This could be done cleaner by creating another form, but then you'll
		// have multiple windows which is harder to manage
		public CardSide(MainMenu menu, Deck deck)
		{
			// This will only initialize the panels
			// Everythnig else must be done manually
			// Fortunately we can copy code from our designer
			continueButton = new Button();
			punishButton = new Button();
			hardButton = new Button();
			goodButton = new Button();
			easyButton = new Button();

			this.continueButton.Location = new System.Drawing.Point();
			this.continueButton.TabIndex = 0;
			this.continueButton.Size = new System.Drawing.Size(75, 23);
			this.continueButton.Text = "Continue";
			this.continueButton.UseVisualStyleBackColor = true;
			this.flip = new System.EventHandler(this.continueToFlip);
			this.toMenu = new System.EventHandler(this.continueToMenu);

			this.easyButton.Location = new System.Drawing.Point(246, 3);
			this.easyButton.Name = "easyButton";
			this.easyButton.Size = new System.Drawing.Size(75, 23);
			this.easyButton.TabIndex = 3;
			this.easyButton.Text = "4 Easy";
			this.easyButton.UseVisualStyleBackColor = true;
			this.easyButton.Click += new System.EventHandler(this.easyButton_Click);

			this.goodButton.Location = new System.Drawing.Point(165, 3);
			this.goodButton.Name = "goodButton";
			this.goodButton.Size = new System.Drawing.Size(75, 23);
			this.goodButton.TabIndex = 2;
			this.goodButton.Text = "3 Good";
			this.goodButton.UseVisualStyleBackColor = true;
			this.goodButton.Click += new System.EventHandler(this.goodButton_Click);

			this.hardButton.Location = new System.Drawing.Point(84, 3);
			this.hardButton.Name = "hardButton";
			this.hardButton.Size = new System.Drawing.Size(75, 23);
			this.hardButton.TabIndex = 1;
			this.hardButton.Text = "2 Hard";
			this.hardButton.UseVisualStyleBackColor = true;
			this.hardButton.Click += new System.EventHandler(this.hardButton_Click);

			this.punishButton.Location = new System.Drawing.Point(3, 3);
			this.punishButton.Name = "punishButton";
			this.punishButton.Size = new System.Drawing.Size(75, 23);
			this.punishButton.TabIndex = 0;
			this.punishButton.Text = "1 Wrong";
			this.punishButton.UseVisualStyleBackColor = true;
			this.punishButton.Click += new System.EventHandler(this.punishButton_Click);

			// Button setup is done. Now do everything else.
			InitializeComponent();
			this.deck = deck;
			studyDone += menu.LoadMenu;
			this.continueButton.Click += this.flip;
			retrieveNextCard();
		}

		private void showContinueButton()
		{
			this.buttonPanel.Controls.Clear();
			this.buttonPanel.Controls.Add(continueButton);
		}

		private void showGradingButtons()
		{
			this.buttonPanel.Controls.Clear();
			this.buttonPanel.Controls.Add(punishButton);
			this.buttonPanel.Controls.Add(hardButton);
			this.buttonPanel.Controls.Add(goodButton);
			this.buttonPanel.Controls.Add(easyButton);
		}

		void retrieveNextCard()
		{
			// No way to reassign an event handler
			this.continueButton.Click -= this.flip;
			showContinueButton();
			try
			{
				this.currentCard = this.deck.nextCard();
			}
			catch (InvalidOperationException ee)
			{
				this.drawref = Deck.emptyMessage;
				this.viewPort.Invalidate();
				this.continueButton.Click += this.toMenu;
				this.deck.save();
				return;
			}
			this.drawref = this.currentCard.content.front;
			this.continueButton.Click += this.flip;
		}

		public void viewPort_Paint(object sender, PaintEventArgs e)
		{
			Single rowPointer = 10;
			float rowSize = font.GetHeight();
			foreach (string text in this.drawref)
			{
				if (text.EndsWith(".jpg") || text.EndsWith(".png"))
				{

				}
				else
				{
					e.Graphics.DrawString(text + "\n", font, Brushes.Black, 10, rowPointer);
					rowPointer += rowSize;
				}
			}
		}

		private void continueToFlip(object sender, EventArgs e)
		{
			this.drawref = currentCard.content.back;
			this.viewPort.Invalidate();
			showGradingButtons();
		}

		private void continueToMenu(object sender, EventArgs e)
		{
			this.Controls.Clear();
			studyDone?.Invoke(this, e);
		}

    private void punishButton_Click(object sender, EventArgs e)
		{
			deck.grade(ref this.currentCard, 1);
			retrieveNextCard();
		}

		private void hardButton_Click(object sender, EventArgs e)
		{
			deck.grade(ref this.currentCard, 2);
			retrieveNextCard();
		}

		private void goodButton_Click(object sender, EventArgs e)
		{
			deck.grade(ref this.currentCard, 3);
			retrieveNextCard();
		}

		private void easyButton_Click(object sender, EventArgs e)
		{
			deck.grade(ref this.currentCard, 4);
			retrieveNextCard();
		}
	}
}
