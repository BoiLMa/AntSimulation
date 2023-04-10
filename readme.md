# **ANT SIMULATION**

## Features
- Ants can find food

- Ants bring back the food to the ant hill

- Bugs spawn on random locations

## Functional
Ants wander around on the screen, when the ant has found food he will walk back the same way he came
Food will spawn as bugs that die and fall on the ground.
A counter on the screen keeps track of all the food collected and how much ants there are currently.

## Technical

When the game starts a for loop starts which instantiates the ants. The ant gameObjects are added to an instance of the Ant
class.
Every ant gets added to a list called antList.
A foreach loop runs in the Update function with a state machine that controls the ants. There are 2 states; Wandering and Collecting.
If the ant is in the wandering state he will pick a random position in a radius of 1.5 when the ant get to this position 
he will pick a new random position and his current position gets saved in a list. This gets repeated until he is close enough 
to a bug. When the ant is close to a bug he will collect food from the bug, this can be done 100 times before the
bug gets destroyed. If the ant has food his state changes to the collecting state. In this state he will go back to the ant hill
following the positions he added to the list on his way to the food. When he arives at the ant hill his state changes back to wandering.
