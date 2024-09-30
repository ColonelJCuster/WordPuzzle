# Word Puzzle

### Description and Features
When this project is run, an MVC application will open up in your default browser. This application will take two user inputs, an integer for the puzzle size, as well as a list of words. 

When the user clicks the Build Puzzle button, a word search puzzle will be randomly generated using the puzzle size and list of words. The puzzle will consist of a matrix measuring
X by X letters, with X being the puzzle size. 

Within that matrix, all words in the provided word list will be hidden in one of 4 directions:
1. Horizontal
2. Vertical
3. Diagonal w/an upward slope (bottom left to upper right)
4. Diagonal w/a downward slope (upper right to bottom left).

The words will be randomly generated using one of the four above directions, picked at random. The words can also be generated forwards or backwards, also picked at random.

Finally, the user has the option to click the Toggle Answers button, which will show the answers, highlighted in green. To remove the highlighting, simply click the Toggle Answers button again.

Enjoy playing around with this Random Word Search Generator!

### Setup Notes
This project was created in Visual Studio 2017 using MVC5, Razor, Bootstrap, and jQuery. As of now, this project has only been run in VS 2017; however, it may or may not run in newer versions of Visual Studio. 
The project runs using IIS Express to serve the page.

To run in Visual Studio, you should be able to simply open the solution file (WordPuzzle.sln), then press F5 or hit the green "play" button at the top of the screen to start execution.
