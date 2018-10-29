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
	public static int WORLD_NO = 1;
	public static string LEVEL_CODE = "B5";
	public static int GAME_DIFFICULTY = 1;  // /100
	public static int THEME = 0;
	public static int GRID_SIZE = 5;
	public static int MAX_TILE = 13;
	public static int PLAYERS = 1;
	public static int TARGET = 100;
	public static int TARGET2 = 150;
	public static int TARGET3 = 175;
	public static int TURNS = 20;
 	public static int TURN_TIME =240;
	public static bool VS_AI = true;
	public static bool VS_LOCAL_MP = false; //make this an enum
	public static bool SOLO_PLAY = false;
	public static int HUMAN_PLAYERS = 1;
	public static int AI_PLAYERS = 1;
	public static bool TUTORIAL_MODE = false;

	public static bool CUSTOM_GAME=false;

	public static int OPPONENT_TILESKIN_1 = 7;
	public static int OPPONENT_TILESKIN_2 = 3;
	public static int OPPONENT_TILESKIN_3 = 2;
	public static int OPPONENT_TILESKIN_4 = 0; //extra tile skin used in the case of a tileskin clash

	public static int[] START_TILE_COUNTS =  new int[] {8,15,8,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0};
	public static int RETURN_TO_WORLD =-1;
	public static int TILESKIN;
	public static int GRIDSKIN = 0;
	public static bool MIRROR_TILESKIN = false;

	public static string Objective1Code = "Rules";
	public static string Objective2Code = "Tutorial";
	public static string Objective3Code = "TurnScore.10"; 

	public static string AI_NAME_1 = "AI 1";
	public static string AI_NAME_2 = "AI 2";
	public static string AI_NAME_3 = "AI 3";
	
}
