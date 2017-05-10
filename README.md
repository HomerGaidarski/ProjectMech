# ProjectMech

A 3D-tank shooter, arcade/survival game made with unity 3D version 5.3.5. 
Waves of enemies spawn via dropships and each wave is progressively harder than the previous wave.

#### Controls
* WASD to move
* Mouse to look and shoot
* 1,2,3 keys to activate powerups
#### Known Issues
* Energy decays quicker before first dropship drops first wave of enemies.
#### Notes
* May take up a few seconds for the game to load after "Play" button is pressed in the main menu or if game is restarted (restart button pressed or player death).
* Exit button in the main menu does nothing in the web build version.
* Recommend PC/Mac build version of this game.
* Playing directly in the main scene in the unity editor will have a brighter scene until the user gets a game over (manual restart via start menu or player dies).


#### Original Game v1.0

Original game created by Homer Gaidarski, Wesley Thompson, Alex Wilhelm, and Joshua Lewis and managed (team meetings/goals/deadlines) by Joseph Griffin as a summer independent study project. Features a third person tank that the player controls and can shoot 1 tank missile every second. There are 2 guns on the tank, and the missile alternates which gun it fires from every time. Tank missiles explode on impact with an explosion radius that does more damage the closer an enemy is to the origin (so you can deal collateral damage and even kill multiple enemies at once). Player also have three powerups they can toggle on at anytime as long as they have energy left (invisibility, speed boost, damage boost). Powerups don’t stack (only 1 can be used at a time). There is no way to heal or regain energy. When player health reaches 0 the player dies and the game shows a game over screen which then restarts the game shortly after. Two enemy types that player can fight: mechs and flying drones. Dropships constantly spawn flying drones and mechs that shoot at you with bullets. Only 30 enemies are on the map at once though, as soon as there are less than 30 enemies on the map, the dropships bring more enemies. Each dropship brings in 4 mechs and 6 drones at a time. There are 4 dropships. The mechs deal more damage and move on the ground, constantly searching for the player. Flying drones deal less damage but they are smaller and float so they are harder to hit.

#### Updated Game v1.5

Updated by Homer Gaidarski, Kenrick Niedbalski, and Jeremy Warden. Made it feel more like a real game. Added a wave system that helps progress difficulty as the game goes on (more enemies each wave). Enemies no long constantly spawn, player only has to fight a certain number of enemies each wave (max enemies on map is now 40 (this ensures use of all 4 dropships each wave), dropships keep bringing more while there are more enemies to spawn for a certain wave). The wave number is displayed on the bottom left of the screen. A score system and display was added as well. The score is displayed in the top left of the screen. Mech kills are worth 100 points while flying drone kills are worth 50. Mechs now drop health packs and drones drop energy packs on death, however they don’t drop every time, there is a 50% chance that the enemy will drop an energy or health pack. This prevents the player from farming infinite energy/health off of enemies while also adding some luck/randomness to the game. Both health and energy packs are rotating cubes, the health packs are green and the energy packs are yellow (matches the colors of the health and energy bars so it is obvious to the player). Increased rate that energy would decay so the powerups aren’t overpowered. Fixed various bugs including one that has existed since the very beginning of ProjectMech (reticle would move off center if the player re-sized the game window while playing).

#### Possible Future Updates:
* Lots of parts of this game use object pooling because instantiating objects at runtime in games (and especially in unity) is very inefficient and slows the game down. But object pooling is not used everywhere it could be. Would be nice to implement object pooling for everything that can/should be pooled to make game more efficient and less laggy.
* A wave does not end until the user kills all enemies, but if an enemy is stuck or the player can’t find the last enemy, this slows the game progression and will bore the player. A good solution would be to create either a mini-map or some sort of x-ray vision that lets the player see enemies through buildings and highlights them (started implementation of this but never finished). Could also solve this by killing last enemy(s) if enemy(s) isn’t moving anymore and is not near player.
* Wave transition is too subtle, when playing in the editor it is obvious because the console and scene editor is visible but for the built version of the game it just changes in the bottom left. Could fix by adding color change or flashing text effects to wave number (with done message). Another fix could be by displaying a larger wave display that fades in and out between waves (already made implementation for this, practically done, just need to handle timing better).
* Special/boss wave. Make it so that every X number of waves there must be a special/boss wave (think of Call of Duty zombies dog rounds). Create pig enemy using pig.obj we were given for graphics class assignments and either make one giant flying boss pig that rains missiles down or a bunch of small pigs.
* Fix energy decaying faster before first wave is dropped problem.
* Add credits screen to main menu showing names of contributors of ProjectMech.
* Swap A-Team music for free license music or make own background music, polish game to perfection, and publish game to some online store such as steam or itch.io or just sell on unity assets store.
* Make mobile phone version.
* Add secondary fire for player, either lock-on rocket(s) or death ray.
