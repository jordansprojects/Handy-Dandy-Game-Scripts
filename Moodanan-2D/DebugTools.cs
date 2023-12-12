using Godot;
using System;

// reference: https://www.reddit.com/r/godot/comments/obxm0i/is_the_godot_script_assert_available_in_c_and_if/
namespace DebugTools{
internal static class Debug  {
	/****************************************************************************
	* Assert : throws exception and error message if boolean condition is not met
	* ***************************************************************************/
	internal static void Assert(bool cond, string msg){
		if (cond) return;
		GD.Print(msg);
		throw new ApplicationException($"Assert Failed: {msg}");
	}
	
} // end of Debug class

}// end of DebugTools namespace

