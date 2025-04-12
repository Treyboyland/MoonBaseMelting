# Moon Base is Melting

## Description

This game is based upon the [print and play version](https://charles-rho.itch.io/moon-base-is-melting) made by Charles Fisher of the same name. There has been some discussion within OGDA about taking some of the board games created in the group and digitizing them either as part of a jam or just for fun. I wanted to see how long that would feasibly take. With the exception of minor bug fixes, the game was completed in about 2 weeks of part-time work.

The rules mirror the rules of the board game, but the starting player is random instead of being the person most likely to stip-mine the lunar surface ~~or the CPU would always go first~~. Play alternates between the player and the CPU for each round. Play continues until all of the pumps of a certain color are consumed by the Ooze 

1. **Move a pump**. A pump must be moved either from the starting area to a pool, among the pools, or from a pool to the starting area. Pumps cannot stack.
2. **Ooze generation**. A random location (group of 8 coordinates and a pool) and a coordinate in that location is rolled 4 times. If a coordinate within a location does not have ooze, it gains ooze. If it already has ooze, the ooze propagates to the 2 adjacent neighbors without ooze. If a location is selected that is fully covered, then the generation steps are repeated for each adjacent location without ooze.
3. **Place ooze**. Place a single ooze on a non-oozed coordinate.
4. **Remove**. Remove ooze from a coordinate, if you have a pump at that location.

## CPU Behavior

Because this is a single-player experience of a game that was originally meant for two players, there is a CPU. This CPU functions as follows:

1. **Move:** The CPU moves pumps out one by one, and will keep them out unless the only valid move is moving the pump back to base.
2. **Place:** The CPU will place ooze where the player has a pump, then in empty locations, and then on its own locations. Potential spots are selected at random.
3. **Remove:** The CPU removes ooze at random from locations where it has a pump.

## Post-Mortem
One issue I have with board games is that there are certain steps that slow down play (e.g. setting up the board; shuffling a deck; determining if any house rules apply, especially for popular games). In this particular instance, I found the ooze generation step the slowest in the board game, and appreciate that this mechanic is handled in by the game now. Of course, this sort of messes with the original conceit of the game:

> "I wanted to try and create an abstracted simulation of the slow, creeping, spreading, and flowing nature of some sort of mutagenic slime. It needed to propagate and represent an ever-growing threat." - Charles Fisher

I feel like now the ooze feels more like an impending eruption or a run-off chain reaction. Since all stages of ooze generation occurs at once, mid-to-late ooze generation feels like the ooze is taking out full swathes of the map each time. I feel like some of the anticipation in location and coordinate selection is lost as well compared to the board game.

Due to personal (and somewhat arbitrary) limitations, the game is single player only. Most of my experience is with single player games, and there were a couple of additional variables that I was unable to design around in that 2 week window (e.g. Does each player have their own cursor, or does control of the cursor switch? How to I abstract the player logic enough so that Player and CPU logical steps are the same? Is this an option that needs to be selected at start, or can the second player join at any time? How do I handle controller disconnects?). Features that got cut for similar reasons include difficulty levels for the CPU (Easy probably would have been random moves. Normal is the current difficuly. Hard would probably need to take into account setting up potential chain reactions with the ooze, as well as optimal ooze and pump placement. What if this Hard difficulty is too hard, and does Easy offer any challenge whatsoever? When is this difficulty chosen?), game setting modifications (would require more UI, determining if I should limit the modifications to some acceptable range), and potential extra modes (e.g. )

I made the art using a combination of GIMP and Aseprite at 16x16. I feel like the art is...serviceable. I would have liked more detailed animations for the pool and pumps, but I am not an artist by any means; creating the sprites as they are probably took me 2 hours. There are also some ideas for "juice" that I ended up abandoning for time (e.g. Having the ooze creep in from the edges as well as the game progressed. Having an ending cinematic of the winning team lifting off of the planets surface as the ooze chased after them. Some type of autonomous robot that would perform the actions instead of things just happening. Having the ooze originate from the pools and sort of "hop" into the appropriate places.) Having an artist for the project would have been nice.

The ooze itself is the core of the game. I probably should have spent more time refining the particle that represents it for both the coordinate and the location overall. A series of multi-colored boxes doesn't really do it justice. Maybe a sprite animation would have been better (but again, time). 

I probably wouldn't recommend trying to digitize a board game within the span of a jam, unless you were extremely experienced with doing so, or you had a system set up that made digitizing them relatively simple (like Table Top Simulator). 