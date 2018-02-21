

//// You have an image capture with build settings and tags used by this pack on ./Demons_pack /Tutorials

-This pack contains an 'ExampleScene' to show you how to create random demons.
-The demons properties can be configured on "Properties.cs", you can configure the head(0-24), body(0-2), gun(0-1) and shield(0-3).
-It contains 4 different rider prefabs. When dies a simply demon will be instantiated.

*About ExampleScene :
	
	-To stop the music go to Main Camera and disable Audio Source.
	-You can easily change the sound effects on Main Camera / Audio_Manager.cs
	-You can select one demon with mouse onclick. He will show a green circle.
	-Only one demon selected at the same time.
	-Demon selected movement:
		-Left mouse button attack
		-a,w,s,d movement
	-Unselect demon by clicking on selected demon.
	-Path points are the childs of 'Path' gameobject, the latest point must be named as 'End'.
	-Demons will be instantiated on 'InstancePoint' gameobject position (Instancer.cs).
	-Demons life configured on Demon_Controller / Rider_Controller.
	-Golems life on Golem_Controller.
	-The golems walk between 2 points (golems childs a, b).	

-You have an example video about making your own path points on ./Tutorials /Making_Path.mp4
	-Remember to clone a gameobject --> ctrl+d.
