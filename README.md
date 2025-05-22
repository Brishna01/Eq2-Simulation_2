# Eq2-Simulation_2
Ce projet permet de simuler des circuits électriques créés sur une grille en utilisant l'analyse nodale modifiée.

## Installation
La version d'Unity utilisée est la version 2022.3.38f1. Pour exécuter le projet, trouver l'exécutable dans le dossier Builds. Sinon, ouvrir le projet sur Unity suivi du menu File > Build and Run.

## Utilisation
### Commandes
- Q: Outil Tracer fils  
- W: Outil Déplacer éléments  
- E: Outil Supprimer fils  
- R: Outil Supprimer éléments
- D: Activer/désactiver le mode débogage  
- Espace: Afficher les matrices dans la console

Lorsqu'on prend un élément de l'interface, l'outil Déplacer éléments est automatiquement sélectionné. Il est possible de mettre un élément en-dehors de la grille pour le mettre de côté et le réutiliser plus tard.

Les paramètres des éléments sont mis à jour chaque fois que le circuit est modifié. Pour afficher les informations d'un élément, simplement mettre la souris sur celui-ci. 

Le mode débogage affiche les noeuds du circuit en numérotant chaque morceau de fil. Un noeud est la région entre deux ou plusieurs éléments et est à la base de la méthode utilisée pour la résolution de circuits. 

## Références
- https://spicesharp.github.io/SpiceSharp/articles/custom_components/modified_nodal_analysis.html  
- https://lpsa.swarthmore.edu/Systems/Electrical/mna/MNA3.html
