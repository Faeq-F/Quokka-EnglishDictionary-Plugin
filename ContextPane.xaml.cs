using Quokka;
using Quokka.ListItems;

using System.Windows;

namespace Plugin_EnglishDictionary {

  /// <summary>
  /// The context pane for file / folder items
  /// </summary>
  public partial class ContextPane : ItemContextPane {

    private readonly DefinitionItem? Item;
    private List<WordPhonetics> phonetics = new();

    /// <summary>
    /// Creates the context pane
    /// </summary>
    public ContextPane() {
      InitializeComponent();
      this.Item = (DefinitionItem) ( (SearchWindow) Application.Current.MainWindow ).SelectedItem!;
      foreach (Phonetic phonetic in Item.phonetics) phonetics.Add(new WordPhonetics(phonetic));
      ButtonsListView.ItemsSource = phonetics;
      WordText.Text = Item.word;
      PartOfSpeech.Text = Item.partOfSpeech;
      Definition.Text = Item.Name;
      Example.Text = Item.example;
      if (Item.synonyms.Count > 0) {
        SynonymsAndAntonyms.Text = "\nSynonyms:\n";
        foreach (String synonym in Item.synonyms) { SynonymsAndAntonyms.Text += synonym + ", "; }
        SynonymsAndAntonyms.Text = SynonymsAndAntonyms.Text.Remove(SynonymsAndAntonyms.Text.Length - 2); //remove last comma & space
      }
      if (Item.antonyms.Count > 0) {
        SynonymsAndAntonyms.Text += "\n\nAntonyms:\n";
        foreach (String antonym in Item.antonyms) { SynonymsAndAntonyms.Text += antonym + ", "; }
        SynonymsAndAntonyms.Text = SynonymsAndAntonyms.Text.Remove(SynonymsAndAntonyms.Text.Length - 2); //remove last comma & space
      }
    }

    private class WordPhonetics {
      public string text { get; set; } = "";
      public string audio { get; set; } = "";
      public WordPhonetics(Phonetic phonetic) {
        this.text = phonetic.text;
        this.audio = phonetic.audio;
      }

    }
  }
}