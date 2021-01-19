LIFPROJET - AM3 (StickMan)
=====================================

Description
-------------------------------------
 
 Projet developpé sous Unity version 2020
 Le jeu se base sur un mode d'infiltration, où le joueur doit pénétrer dans un bâtiment et éliminer ses ennemis, en traversant diverses pièces. Il peut aussi effectuer diverses actions : Marche lente, Course, Saut, Accroupissement, Changement d’arme (poing, pistolet).
Le joueur dispose d’un pistolet qui lui permet de tirer sur les ennemis, il peut aussi choisir d’utiliser ses poings pour frapper ses adversaires. Il peut interagir avec l'environnement, se cacher derrière le décor (une caisse) ou bien rentrer dans un placard, pour éviter que l’ennemi ne le détecte. 

    
Environement
-------------------------------------
Fonctionne sous Windows 10 

Files
-------------------------------------
```
Red
|           README.md
|-----------Assets
            |-------Audio
            |-------Imports
            |-------Pallettes
            |-------Physics
            |-------Resources
                    |-------animation
                            |-------Animation
                                    |-------Black
                                    |-------Red
                                    |-------Closet
                                    |-------Door
                            |-------Animator
                                    |-------rewind (animators qui contient animations renversées)
                    |-------gameObjects
                    |-------sprite
            |-------Scenes
            |-------Script  
            |-------TextMeshPro (asset du unity)
|-----------Executable
            |   
|-----------Package (Tous les extension du unity)
|-----------ProjectSettings
            |   executable
```

Lancement du jeu 
-------------------------------------

Pour lancer le jeu, le fichier executable se trouve dans le répertoire suivant : SND/Executable/ProjectSettings/executable

Mode de jeu 
-------------------------------------

Au lancement du jeu, l'utilisateur se retrouvera au menu principal avec 3 choix disponibles,
Start, Settings, Quit.

+ Start - Deux modes de jeux sera disponible :
    + Story : Comporte deux niveaux (Stage1, Stage2)
    + Survival : Niveau bonus
+ Settings - 
     + Choisir la résolution
     + Activé/Désactivé mode plein ecran
     + Réglage du volume
+ Quit - Permet de quitter le jeu 

Usage / controle
-------------------------------------

Dans ce jeu, on a 2 mode : 
+ Story mode : Traverser les niveaux jusqu'à atteindre la room final
+ Survival mode :  Objective de ce mode est de survivre le plus longtemps possible


Pour controller le personnage Red :

* Z - sauter
* A,D - ces touches sont les deplacements horizontals.
* S - s'accroupir quand Red est près  d'une boite
* Clique Gauche - pour tirer le pisotel
* Espace - interaction avec des objets (les portes et les armoire)
* Souris - pour viser 
* T - active le rewind 

