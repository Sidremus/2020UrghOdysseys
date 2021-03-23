# 2020UrghOdysseys

This is a Jam Project, submitted here:
https://sidremus.itch.io/urghodyssey

Note that for since I don't hold exlusive rights to the music and sound files, I omitted them here.

The overall architecture resembles an Entity Component System (ECS). Working in Unity, this is clearly the easiest structure to follow, as it ties in nicely with the component workflow that's at the heart of Unity.

The project also makes use of several design patterns, most notably the Command pattern, which is responsable for for switching major game states. The Observer patterns is also used to a lesser degree, as there are some relatively passive scripts that handle high-scores and spawing in new waves of enemies.

![alt text](https://github.com/Sidremus/2020UrghOdysseys/blob/main/CoronaFighterGalactic07_PlayerShip.gif)
