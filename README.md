# 🐉 Dragons Race VR

Un jeu de course de dragons en réalité virtuelle avec intelligence artificielle et plateforme de mouvement physique.

![Unity](https://img.shields.io/badge/Unity-2022.3+-blue.svg)
![C#](https://img.shields.io/badge/C%23-8.0+-green.svg)
![VR](https://img.shields.io/badge/VR-Supported-purple.svg)
![AI](https://img.shields.io/badge/AI-ML%20Agents-orange.svg)

## 🎮 Aperçu du Jeu

**Dragons Race VR** est une expérience de course immersive où vous pilotez un dragon dans un environnement VR avec des adversaires contrôlés par IA. Le jeu intègre une plateforme de mouvement physique pour une immersion totale.

### 🎥 Démonstration (24 secondes)

[![Vidéo de démonstration](https://img.shields.io/badge/🎥-Voir%20la%20démonstration%20(24s)-red)](https://github.com/user-attachments/assets/8bd49211-f5c2-4ec0-b4e3-a487ad564925)

*Durée : 24 secondes - Montre le gameplay complet avec VR et IA*

## ✨ Fonctionnalités

- 🤖 **Intelligence artificielle** avec Unity ML-Agents
- 🎯 **Système de combat** avec projectiles
- 🏆 **Bonus et obstacles** dynamiques
- 🎮 **Contrôles VR** immersifs
- 🔄 **Plateforme de mouvement** physique (ForceSeat)
- 🗺️ **Système de checkpoints** intelligent

## 🛠️ Technologies Utilisées

- **Unity 3D** - Moteur de jeu
- **Unity ML-Agents** - Intelligence artificielle
- **ForceSeatMI** - API plateforme de mouvement
- **C#** - Langage de programmation
- **VR SDK** - Support réalité virtuelle

## 📁 Structure du Projet

> 📝 **Note** : Ce repository contient uniquement les scripts C#. Les assets 3D (modèles, textures, sons) doivent être créés séparément.

```
Scripts/
├── DragonController.cs              # Contrôleur principal du dragon (3D complet)
├── DragonControllerOnlyHorizontal.cs # Contrôleur simplifié (course horizontale)
├── GameManager/
│   └── GameManager.cs              # Gestionnaire principal du jeu
├── DagonIAScript/
│   └── DragonIAObstacle.cs         # Agent IA avec ML-Agents
├── Shoot/
│   ├── Shoot.cs                    # Système de tir automatique IA
│   ├── MyShoot.cs                  # Tir contrôlé par le joueur
│   └── Fireball.cs                 # Projectile de combat
├── Bonus/
│   └── BonusScript.cs              # Gestion des bonus de vitesse
├── Checkpoint/
│   ├── TrackCheckpoints.cs         # Système de checkpoints
│   └── CheckpointSingle.cs         # Checkpoint individuel
├── Platform/
│   ├── Platform.cs                 # Intégration ForceSeat complète
│   └── PlateformOnlyHorizontal.cs  # Version simplifiée
├── Horizontal.cs                   # Navigation intelligente avec raycasting
└── SceneSwitch.cs                  # Changement de scènes
```

### 🎨 Assets Requis (à créer vous-même)

- **Modèle de dragon** avec animations de vol
- **Terrain de course** ou circuit
- **Obstacles** (rochers, murs, etc.)
- **Bonus** (objets collectibles)
- **Checkpoints** (portails ou marqueurs)
- **Environnement** (ciel, textures, etc.)

## 🚀 Installation et Configuration

> ⚠️ **Important** : Ce repository contient uniquement les **scripts C#**. Vous devez créer vos propres assets 3D (dragons, environnement, etc.) et configurer la scène Unity vous-même.

### Prérequis

1. **Unity 2022.3+**
2. **VR Headset** (Oculus, HTC Vive, etc.)
3. **ForceSeat Platform** (optionnel)
4. **Manette VR** ou **Manette standard**
5. **Assets 3D** : Modèles de dragons, environnement, obstacles

### Installation

1. **Cloner le repository**
   ```bash
   git clone https://github.com/VOTRE_USERNAME/dragons-race-vr.git
   cd dragons-race-vr
   ```

2. **Créer un nouveau projet Unity**
   - Ouvrir Unity Hub
   - Créer un nouveau projet 3D
   - Nommer le projet "DragonsRaceVR"

3. **Importer les scripts**
   - Copier le dossier `Scripts/` dans votre projet Unity
   - Les scripts seront automatiquement compilés

4. **Installer les dépendances**
   - Unity ML-Agents (Package Manager)
   - XR Plugin Management (Package Manager)
   - Packages VR selon votre casque

## 🎮 Configuration VR

### Setup Oculus/Meta Quest

1. **Installer Oculus Link/Air Link**
2. **Configurer Unity XR**
   ```
   Window > Package Manager > Unity Registry > XR Plugin Management
   ```
3. **Ajouter le provider Oculus**
   ```
   Project Settings > XR Plug-in Management > Oculus
   ```

### Setup HTC Vive

1. **Installer SteamVR**
2. **Configurer OpenVR dans Unity**
   ```
   Project Settings > XR Plug-in Management > OpenVR
   ```

### Contrôles VR

- **Mouvement** : Stick analogique gauche/droite
- **Tir** : Boutons 4 et 5 simultanément
- **Démarrage** : Boutons 4 et 5 pour commencer

## 🔧 Configuration Plateforme de Mouvement

### ForceSeat Installation

1. **Installer ForceSeatPM**
   - Télécharger depuis [motionsystems.eu](https://www.motionsystems.eu)
   - Installer le logiciel ForceSeatPM

2. **Configuration Unity**
   ```csharp
   // Le script Platform.cs se connecte automatiquement
   // Vérifiez que ForceSeatPM est en cours d'exécution
   ```

3. **Paramètres de la plateforme**
   - **Roll** : -16° à +16° (mouvement latéral)
   - **Pitch** : -16° à +16° (mouvement avant/arrière)
   - **Heave** : 0 à 1.0m (mouvement vertical)

### Test de la Plateforme

```csharp
// Dans Platform.cs, vérifiez que :
Debug.Log("L'objet a été créé"); // Doit apparaître dans la console
```

## 🤖 Configuration IA

### Unity ML-Agents

1. **Installation**
   ```bash
   pip install mlagents
   ```

2. **Entraînement**
   ```bash
   mlagents-learn config.yaml --run-id=dragon-race
   ```

3. **Configuration des récompenses**
   - **Checkpoint correct** : +100 points
   - **Collision obstacle** : -1000 points
   - **Collision mur** : -200 points
   - **Bonus collecté** : +10 points

## 🎯 Configuration des Assets

### Création de la Scène

1. **Créer un terrain de course**
   - Ajouter un terrain ou des plateformes
   - Créer un circuit avec des virages
   - Ajouter des obstacles (rochers, murs)

2. **Importer un modèle de dragon**
   - Trouver un modèle 3D de dragon (Asset Store, Sketchfab, etc.)
   - L'importer dans Unity
   - Configurer les animations (vol, rotation)

3. **Configurer le dragon joueur**
   ```csharp
   // Ajouter ces composants au GameObject dragon :
   - DragonController.cs OU DragonControllerOnlyHorizontal.cs
   - Rigidbody (avec constraints appropriées)
   - Collider (Capsule ou Mesh)
   - Animator (si animations disponibles)
   - Tag: "MyDragon"
   ```

4. **Configurer les dragons IA**
   ```csharp
   // Pour chaque dragon IA :
   - DragonIAObstacle.cs
   - Rigidbody
   - Collider
   - Tag: "DragonIA"
   ```

5. **Créer les objets interactifs**
   ```csharp
   // Bonus de vitesse :
   - BonusScript.cs
   - Collider (Trigger)
   - Tag: "Bonus"
   
   // Obstacles :
   - Collider
   - Tag: "Rock" ou "Wall"
   
   // Checkpoints :
   - CheckpointSingle.cs
   - Collider (Trigger)
   ```

6. **Ajouter le GameManager**
   ```csharp
   // Créer un GameObject vide :
   - GameManager.cs
   - Nommer "GameManager"
   ```

## 🎮 Utilisation

### Démarrage du Jeu

1. **Configurer la scène** (voir section Configuration des Assets)
2. **Appuyer sur Play** dans Unity
3. **Utiliser les boutons 4+5** pour démarrer la course

### Modes de Jeu

- **Mode Solo** : Course contre IA uniquement
- **Mode Multijoueur** : Jusqu'à 8 joueurs
- **Mode Entraînement** : Pour tester les contrôles

### Contrôles

| Action | Contrôle |
|--------|----------|
| Mouvement gauche/droite | Stick analogique horizontal |
| Tir | Boutons 4 + 5 (manette) |
| Démarrage | Boutons 4 + 5 |
| Redémarrage | Touche R |

## 🏗️ Architecture Technique

### Système de Contrôle

```csharp
// DragonController.cs - Mouvement 3D complet
- Mouvement horizontal (rotation Y)
- Contrôle d'altitude (mouvement Y)
- Rotation réaliste (pitch/roll)
- Animations de vol
```

### Intelligence Artificielle

```csharp
// DragonIAObstacle.cs - Agent ML-Agents
- Observation de l'environnement
- Système de récompenses/pénalités
- Évitement d'obstacles
- Apprentissage par renforcement
```

### Système de Combat

```csharp
// Shoot.cs - IA de tir
- Raycast pour détection d'ennemis
- Logique floue pour décision de tir
- Gestion du taux de tir
- Projectiles avec physique réaliste
```

## 🐛 Dépannage

### Problèmes de Configuration

**Scripts non fonctionnels :**
- Vérifier que tous les composants requis sont ajoutés
- Vérifier les tags des GameObjects ("MyDragon", "DragonIA", etc.)
- Vérifier que les Colliders sont configurés correctement

**Dragon ne bouge pas :**
- Vérifier que le Rigidbody est attaché
- Vérifier les constraints du Rigidbody
- Vérifier que le script DragonController est attaché

### Problèmes VR

**Pas de détection du casque :**
- Vérifier les drivers VR
- Redémarrer Unity après connexion
- Vérifier les paramètres XR

**Contrôles non fonctionnels :**
- Vérifier la configuration des inputs
- Tester avec une manette standard
- Vérifier les boutons dans Input Manager

### Problèmes Plateforme

**ForceSeat non détectée :**
```csharp
// Vérifier dans la console Unity :
Debug.LogError("ForceSeatMI library has not been found!");
// Solution : Installer ForceSeatPM
```

**Mouvements incorrects :**
- Vérifier la calibration de la plateforme
- Ajuster les constantes dans Platform.cs
- Vérifier les limites physiques

## 📊 Performance

### Optimisations

- **Raycasting** : Limité à 180 unités
- **Collisions** : Système de tags optimisé
- **Animations** : États booléens simples
- **Physique** : Rigidbody avec constraints

### Recommandations

- **GPU** : GTX 1060 minimum
- **RAM** : 8GB minimum
- **CPU** : Intel i5 ou équivalent AMD

## 💡 Ressources Utiles

### Modèles de Dragons
- **Unity Asset Store** : Rechercher "Dragon" ou "Fantasy Creatures"
- **Sketchfab** : Modèles 3D gratuits et payants
- **Mixamo** : Animations de créatures fantastiques

### Tutoriels Unity VR
- [Unity VR Development](https://learn.unity.com/tutorial/vr-development)
- [XR Plugin Management](https://docs.unity3d.com/Packages/com.unity.xr.management@latest/)

### ForceSeat Documentation
- [ForceSeatPM Manual](https://www.motionsystems.eu/support/)
- [ForceSeatMI API Reference](https://www.motionsystems.eu/support/)

## 🤝 Contribution

Les contributions sont les bienvenues ! Voici comment contribuer :

1. **Fork** le projet
2. **Créer** une branche feature (`git checkout -b feature/AmazingFeature`)
3. **Commit** vos changements (`git commit -m 'Add some AmazingFeature'`)
4. **Push** vers la branche (`git push origin feature/AmazingFeature`)
5. **Ouvrir** une Pull Request

### Idées d'Améliorations
- Nouveaux types d'obstacles
- Système de power-ups avancé
- Modes de jeu supplémentaires
- Optimisations de performance
- Support de nouveaux casques VR

## 👨‍💻 Auteur

**Hicheme BEN GAIED**
---

⭐ **N'hésitez pas à donner une étoile si ce projet vous a aidé !**
