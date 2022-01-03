using System;
using System.Windows.Forms;
using System.Text.Json;
using deck;

namespace WinFlash
{
  public partial class MainMenu : Form
  {

    public MainMenu()
    {
      InitializeComponent();
      Directory.CreateDirectory(Deck.saveFolder);
      this.deckBox.SelectionMode = SelectionMode.One;
      foreach (string d in Directory.EnumerateDirectories(Deck.saveFolder))
      {
        this.deckBox.Items.Add(Path.GetFileNameWithoutExtension(d));
      }
    }

    public void LoadMenu(object sender, EventArgs e)
    {
      this.Controls.RemoveAt(0);
      this.Controls.Add(this.studyButton);
      this.Controls.Add(this.deckBox);
    }

    private void MainMenu_Load(object sender, EventArgs e)
    {
      this.AutoSize = true;
    }

    private void studyButton_Click(object sender, EventArgs e)
    {
      // Initialize our deck
      Deck? deck;
      try
      {
        deck = new Deck(this.deckBox.SelectedItem.ToString());
      }
      catch (FileNotFoundException ex)
      {
        MessageBox.Show(this, "Please go into %appdata%\\WinFlash and add it",
          "No JSON found for this deck.",
          MessageBoxButtons.OK, MessageBoxIcon.Error);
        return;
      }
      catch (JsonException ex)
      {
        MessageBox.Show(this,
          "Please check your JSON to ensure it's made correctly.",
          "JSON Read Error", MessageBoxButtons.OK,
          MessageBoxIcon.Error);
        return;
      }

      this.Controls.Clear();
      this.Controls.Add(new CardSide(this, deck));
    }

    // Instead of having two usercontrols for the front and back sides
    // of the card, let's make a single user control for making card sides,
    // and another control within that for displaying the buttons.
    // That way we can allocate a single panel for the entirety of the session
    public void showCardSide()
    {
    }

    public void clearForm(object sender, EventArgs e)
    {

    }

    private void deckBox_SelectedValueChanged(object sender, EventArgs e)
    {
      this.studyButton.Enabled = true;
    }
  }
}
