# 2020UrghOdysseys

This is a Jam Project, submitted here:
https://sidremus.itch.io/urghodyssey

Note that since I don't hold exclusive rights to the music and sound files, I omitted them here.

The overall architecture resembles an Entity Component System (ECS). For me, when working in Unity, this the easiest structure to follow. It ties in nicely with the component workflow that's at the heart of Unity.

There are inklings of several design patterns here. The Command pattern, which is responsable for for switching major game states. The Observer patterns is also used to a lesser degree, as there are some relatively passive scripts that handle high-scores and spawing in new waves of enemies. 

![alt text](https://github.com/Sidremus/2020UrghOdysseys/blob/main/CoronaFighterGalactic07_PlayerShip.gif)
