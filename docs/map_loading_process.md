#  Map dev 
Back to [Main read me](../README.md)



The terrain is created based on `.Assets/Resources/height_map.png` wich is used as a displacement map.

In a futur iteration terrain will be generated from province type

The canvas is created based on the size of the `Province_Map.png` image.
Each province is positioned on the canvas using the `lowerX` and `higherY` values, which represent the top-left corner of the sprite. These values are extracted from the map position JSON file (`.Assets/Resources`), which is generated during the map creation phase.

[What is displacement mapping ?](https://www.creativebloq.com/features/a-beginners-guide-to-displacement-and-bump-maps)
