public class ApplicationModel {
	

	/// 
	/// These values all contain defaults for quick play testing only, 
	/// Games are all configured using the game_configuration.cs class
	///  This application model simply acts as a mediator of game configuration
	/// settings from title screen scene and the main gameplay scene.
	///


	
	public static bool MUSIC_ENABLED = true;
	public static bool FX_ENABLED = true;
	public static int LEVEL_NO = 0;
	public static int WORLD_NO = 0;
	public static string LEVEL_CODE = "";
	public static int GAME_DIFFICULTY = 20;  // /100
	public static int THEME = 0;
	public static int GRID_SIZE = 7;
	public static int MAX_TILE = 19;
	public static int PLAYERS = 1;
	public static int TARGET = 123;
	public static int TARGET2 = 222;
	public static int TARGET3 = 333;
	public static int TURNS = 20;
 	public static int TURN_TIME =330;
	public static bool VS_AI = true;
	public static bool VS_LOCAL_MP = false; //make this an enum
	public static bool SOLO_PLAY = false;
	public static int HUMAN_PLAYERS = 1;
	public static int AI_PLAYERS = 1;
	public static bool TUTORIAL_MODE = false;

	public static int[] START_TILE_COUNTS =  new int[] {15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15};
	public static int RETURN_TO_WORLD =-1;
	public static int TILESKIN;
	public static int GRIDSKIN = 0;
	public static bool MIRROR_TILESKIN = false;

	public static string Objective1Code = "Swaps.5";
	public static string Objective2Code = "SwapsWin.5";
	public static string Objective3Code = "Errors.0"; //less than

	public static string AI_NAME_1 = "AI 1";
	public static string AI_NAME_2 = "AI 2";
	public static string AI_NAME_3 = "AI 3";
	
}
