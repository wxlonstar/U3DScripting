

Hello! This is Bezier Master! With it, you can easily create 3D curves and place objects along it!


To do that:

1. Create Bezier Master object in Tools/Create Bezier Master menu, or just add Bezier Master component on your object;


2. Manipulate controll points, add or remove points to create curve that you need. 
You can also change rotation and scale for each point in Curve Editor tab. That will affect instatiated objects;

2.1 Copy/Paste buttons at Curve Editor copy and paste curve information like points, rotations, scales etc.


3. In Create Objects tab you can choose Objects or Mesh to create and parameters to it 
(like Prefabs, that you want place (it can be more than 1), mesh resolution etc.)

3.1 Copy/Paste buttons at Create Objects tab copy and paste mesh generating parameters and can copy betwen many Bezier Curves;

3.2 Make Prefab button creates mesh asset at top of Assets folder.


4. Also you can create just curve (choose None at top of Objects Instatiating tab) and get points along it to move or something else.
Just call GetPath(pointsCount) from object with that component. It returns array of Vector3;


5. See the Demonstration scene and Example scripts to understand how it works!


6. You can also use other plugin (like FBX Exporter) to create a fbx from generated mesh and import it in 3D modeling software. 
When mesh created press Detach Mesh button and export obtained game object.



Hope that will be useful for you!
With any questions and suggestions you can contact me by email: newbeedevinc@gmail.com

Alexander Kotov


Many Thanks to Jasper Flick https://catlikecoding.com for Curves And Splines Tutorial and Mesh Generating Tutorials. 


v1.1 changes
- fix some bugs

v1.2 changes: 
- fixed some errors, some base architecture change. Can cause errors on update from early ver.
- now curves are dynamic! See example scripts.

v1.21 changes:
- Insert point feature
- Tooltips
- UI improve