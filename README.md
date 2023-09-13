<h1 align="center">
  Decks of Randomness Notecards in WPF
</h1>

## About this app

Anki is better notecard app, but this problem showed me how powerful programming can be, so I wanted to create a notecard app.



Decks of Randomness Notecards is a notecard app that allows you to group notecards by topic, but still be able to share a notecard between different groups. This makes it ideal for studying languages, where words can have multiple meanings or uses.

The app was originally designed to help people learn Japanese, where words can have multiple kanji (Chinese characters). For example, the word "to read" can be written as 読む (yomu) or 読める (yomieru). By grouping notecards by kanji, you can make connections between the different meanings of a word.

Decks of Randomness Notecards also allows you to randomize your notecards by group or by word. This helps you to avoid memorizing the order of your cards and to focus on the actual content.


## Features
* Randomly order groups and facts/words.
 ![Picture of random notecard view](/assets/RandomlyOrders.jpg)
* Select groups to be included in the random pool.
* Add categories, groups, and facts.
* Specific to Japanese learning:
 * Highlight kanji in a word that you have not yet added.
 ![Picture of kanji being highlighted in red](/assets/HighlightIfNotAdded.jpg)
 * When adding a new kanji, automatically add it to all words that contain it.
   * Added 体 for picture
  ![Picture of kanji from previous picture not being red](/assets/AddedKanjiNowNotHighlighted.jpg)
 * When adding a new word, automatically add it to all kanji that are used in it.

## Install
Will make a sqlite database in %LOCALAPPDATA%\zzNihongoDb

Install Visual Studio, clone repo, and press the run button.

## Recommendations

* I like wanikani for learning kanji atm
* I like Anki for notecards.

## Additional ideas