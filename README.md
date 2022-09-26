# MondoMegabits NFT Trading Card Game
Mondo Megabits NFT trading card game running as a 3D tabletop card game built in Unity.

### Welcome
This is an alpha build of the game. It is a sandbox version with no enforced rules or card placement. Some card types are missing for game play, this will be fixed as soon as we have them available to add. A lot still needs to be added. In the meantime, have fun.

Please report all issues and make feature requests.

## Gameplay
### Audio
In the alpha build there is no game audio or player audio. Please use discord for chat.

### Multiplayer
You can either connect to the gameserver that is prepopulated, or change...
>Address: localhost

in the network HUD upon game startup. If you host a game you can play locally with other machines, or give your friend your [IP address](https://whatismyipaddress.com/) for them to input to connect as a player (this requires port 7777 open on your router for them to ocnnect in to).

You connect in as a spectator, up to 100 per server, and any 2 people can sit down to play against eachother.

### The Game
You start playing by walking up to one of theplayer chairs. You will be locked in for play until you press the red exit button.

To start play you simply click on the deck, you will be asked to input a MondoDeckFile to build your deck from. There are some included with the game, they are easy to make. You put the number followed by a | and then card name (card names must match the card name found in this file or the deck wont show those card faces)

Prebuilt deck files for use:
>Assets/Resources/MondoDeckFiles/

#### Planet B.mm
	3|Electric Chairman
	3|Startup Guy
	3|AI Overlord
	3|Blockchain Evangelist
	3|Understandroid
	3|Lushsux Dix
	3|Authentic World
	2|Imposter Moon
	2|The Living House
	3|Planet B
	2|Panopticon You
	3|Rogue Implant
	2|Nopalgarthian Emplacer
	3|Hot Potato (Nanite Swarm)
	2|Crapto Currency
card names must match those found in this file
>Assets/Resources/CardFiles/MondoCardsJSON.json

or they will have no card face in game.

## Unity Building
In order to run/edit/build the game in Unity you need to populate the folder below
> Assets/Resources/CardFiles/fullres/

with the {cardId}.mp4 files in this link, [FullRes card mp4 files.](https://drive.google.com/drive/folders/1jOW-8CFJgBwAQ8ribjDxyOZKc2DSAbZC?usp=sharing "FullRes mp4s")

### Thanks
Author: rttgnck