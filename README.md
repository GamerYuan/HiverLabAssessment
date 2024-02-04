## README

Unity Repository: https://github.com/GamerYuan/HiverLabAssessment
Backend Repository: https://github.com/GamerYuan/HiverLabAssessmentBackend

## Controls

- `WASD` to move
- `Space` to jump
- `Left Click` to 
- `Mouse` to look around

## Gameplay

- This is a simple arcade-style shooter game where the player has to survive the zombie horde.
- The player starts with 20 HP and loses HP when hit by a zombie.
- The player can shoot the zombies to kill them and score points.

### Item Packs

- There are 2 typs of item packs in the game:
	- Health Pack: Restores 10 HP
	- Ammo Pack: Increases damage of bullet

### Zombies

- Zombies spawn in waves and increase in number with each wave.
- Zombies move mindlessly until player is in their radius of detection.
- Zombies move towards the first position where the player was detected.
- Zombies chase the player when in close approximity, or when shot.
- Zombies attack the player when in attack range.
- Zombies have more HP and deal more damage as time progresses.
- Killing zombies rewards the player with scores, which increases based on the HP of the zombie.

### Game Over

- The game ends when the player's HP reaches 0.

## Features

### Leaderboard

- A custom leaderboard system is implemented using a custom backend API.
- Players can submit their scores to the leaderboard when the round ends.
- The top 30 scores will be stored and displayed on the leaderboard.

Backend API: https://hiverlab-assessment-backend.yuan0801.workers.dev/

- The backend API is hosted on **Cloudflare Workers**.
	- The restrictions of Cloudflare Workers Free tier includes 100,000 read requests and 1000 write requests per day.
- The API is a simple REST API with 2 endpoints:
	- `GET /highscores` to get the top 30 scores
	- `POST /newscore` to submit a new score
		- Parameters:
		- `name` (string): The name of the player
		- `score` (int): The score of the player

### Animated Grass

- The grass is animated using a custom shader.
- The shader is implemented with Unity's URP Shader Graph
- The shader uses the `Wind` node to simulate the movement of the grass.

### Blood Splatter

- Zombies splatter blood when shot.
- The blood splatter is implemented using Unity's VFX Graph.
- It uses a custom shader to simulate the texture of blood splatter.

## Assets

These are the list of free assets used in the game:
- [Zombie by pxltiger](https://assetstore.unity.com/packages/3d/characters/humanoids/zombie-30232)
- [Terrain Sample Asset Pack by Unity Technologies](https://assetstore.unity.com/packages/3d/environments/landscapes/terrain-sample-asset-pack-145808)
- [First aid kit army by Game Stuff Studio](https://assetstore.unity.com/packages/3d/props/first-aid-kit-army-148353)
- [Low Poly Environment by Polytope Studio](https://assetstore.unity.com/packages/3d/environments/lowpoly-environment-nature-free-medieval-fantasy-series-187052)
- [Free Night Sky by qianyuez](https://assetstore.unity.com/packages/2d/textures-materials/sky/free-night-sky-79066)
- [Zombie Sound Pack by Catastic](https://assetstore.unity.com/packages/audio/sound-fx/zombie-sound-pack-free-version-124430)
- [Rifle Guns Sound Effect by WOW Sound](https://www.youtube.com/watch?v=3Y1F3ZcDpSM)