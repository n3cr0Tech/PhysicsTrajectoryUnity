# REFERENCE:
## TNTC from Youtube
## https://www.youtube.com/watch?v=4VUmhuhkELk&t=122s

# HOW IT WORKS
## The GameManager generates the cannon and the floor then tells TrajectorySim to copy them onto the physics simulation scene.
## The physics simulation scene is what's being used to calculate the line renderer's path, so anything that happens on the SampleScene has to also happen in the simulation scene in order to have an accurate trajectory displayed
## To increase the length of the trajectory, increase the number of iterations done by TrajectorySim

# PROPERTIES YOU COULD TWEAK
## Amount of iterations for Unity to calculate the ball's trajectory (can be changed in TrajectorySim)

# HOW TO USE
## Arrow keys control the cannon's rotations
## Spacebar shoots the ball out