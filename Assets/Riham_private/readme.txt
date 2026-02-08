// First: movement:
//     1. add Rigidbody.
//     2. add player_movement(code) file to the player and add the player into the rb in the inspector.
//     the player will move to left, right, up, down using the arrows or (w,a,s,d) toetsen.

// Second: item collecting:
//     1. add Player_inventory(code) to the player.
//     2. add items(code) to the item I want to collect. 
//     3. make a collider for the wanted items.
//     4. make a collider for the player if it is not exist and tik the trigger.
//     5. notes:
//         i mada an empty element as a child of the player to make a collider and i make it onTrigger to avoid go through the walls

// Third: adding items counter in the screen of the player:
//     1. add canvas :
//         1.1. make by canvas scaler -> ui scale mode : scale with screen size (to get changeble with the size of the screen).
//         1.2. by reference resolution x=1920, y=1080 (kan veranderen).
//     2. add (under canvas) text-textmeshpro:
//         2.1. by react transform I can change where i want the text that i added to apear on the screen (by the box press shift+alt).
//     3. add ui--> image:
//         3.1. when choose image we can import new assets from the computer but make shore that by the texture type in the foto is on sprite(2d and ui).
//         3.2. by image inspector go to image and choose for source image the foto that you want (kan zijn de uploaded foto van computer).
//     4. UI_inventory(code) to text element.
//     5. add by Player_inventory in the inspector of the player new list and then drag the text elemnt to the list and choose the function of update items (kan anders nemen).

// Fourth: switch levels:
//     1. create empty object and add to it next_level(code).
//     2. make by player in player inventory in inspector in list new element and add the empty object that you create.
//     3. add fucntion of next_level 
//     (that will make the game ending after collecting two items to change that you can change the code of next_level)

// Fivth: inventory (to make the object go to next level )
//     1. add the same_object(code) to this object that you want to save

// useful_links:
//     https://www.youtube.com/watch?v=EfUCEwKmcjc  (collecting items)