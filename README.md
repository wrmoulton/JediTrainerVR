# Jedi Trainer VR

## Overview
Jedi Trainer VR is a virtual reality lightsaber training simulator designed to help young Jedi develop combat and Force skills. The game features multiple combat modes, Force powers, visual and audio effects, and wave-based enemy encounters culminating in a challenging boss fight.

This project was developed in Unity as part of a team assignment.

---

## How to Run the Game

1. Clone the repository from:  
   https://github.com/wrmoulton/JediTrainerVR.git

2. Open **Unity Hub**

3. Add the cloned project folder

4. Open the project using the recommended Unity version

5. Connect your VR headset (Meta Quest via Link or equivalent)

6. Press **Play** in the Unity Editor or build the project to your headset

---

## Game Modes

### Mode 1: Training Droid Mode
- A training droid moves randomly (or pseudo-randomly)
- Fires lasers at varying times
- Player deflects lasers using a lightsaber
- Focuses on timing and defensive skills

### Mode 2: Attack Droid Combat Mode
- Waves of attack droids approach the player
- Player does **not** move through the environment
- Combat relies on lightsaber and Force abilities
- After several waves, a **boss droid** appears that is significantly harder to defeat

---

## Force Powers Implemented

The following Force powers are available in the game and all take up some force meter:

1. ⚡ **Force Lightning**  
2. 🧲 **Object Grabbing & Throwing** *(implemented but currently buggy)*  
3. 🌊 **Force Wave** – Knocks enemies backward  
4. 💚 **Force Heal** – Restores player health  
5. 👁️ **Force Foresight** – See into the future (enemy prediction mechanic)
   - Note about this is that its hard to see in the video, but a transparent droid is shown where the enemy will travel
7. 🔥 **Call the Force from All Jedi**  
   - Grants **dual lightsabers**
   - Emits intense Force energy
   - Completely drains the Force meter
   - Intended **only for the boss fight**

---

## UI & Feedback Systems

- Health meter
- Force meter
- Game End Screen
- Visual feedback for Force usage
- Enemy wave tracking
- Score tracking for Training Droid mode
- Game-over state handling


---

## Audio & Visual Effects

- Lightsaber special effects (visual glow and impact effects)
- Background music
- Sound effects for combat
- Emphasis on immersion and player feedback
- Sound Effect when player was being hurt

---

## Strategy & Design Decisions

- Force powers are mapped to intuitive controller gestures and inputs
- Wave-based enemy design encourages pacing and escalation
- Boss fight requires careful Force resource management
- Player remains stationary to emphasize reaction time and combat skill
- Visual clarity prioritized for VR comfort and usability

---

## Controls

### Lightsaber
- **Grab Lightsaber:** Press the **grab button** on either controller  
- Lightsaber movement directly follows controller motion

### Force Powers
- **Force Lightning:** Press **A**  
- **Force Grab:** Point the **top of the controller** at a grabbable object  
- **Force Wave:** Press **Y** and push the controller forward  
- **Force Heal:** Press **X** and shake the left controller  
- **Force Foresight:** Press **B** on the right controller  
- **Call the Force from All Jedi:** Press **both grab buttons** and lift both hands upward  
  - Grants dual lightsabers
  - Fully drains the Force meter
  - Intended for boss encounters

### UI Feedback
- Health and Force meters display current player status
- Visual and audio cues indicate successful or unavailable ability usage

---

## Known Bugs & Limitations

- **Throwable objects are buggy**
  - Physics interactions can behave inconsistently
  - Object selection is not always reliable

- **Enemy hitboxes can be inconsistent**
  - Lightsaber collisions do not always register correctly
  - Occasionally results in missed or delayed damage

These issues are known and were not fully resolved within the project timeline.

---

## Team Contributions

### My Contributions
- Implemented **Attack Droid Combat Mode**, including the core scripting logic that controls enemy spawning, movement, and behavior as droids actively advance toward the player
- Implemented Force Powers:
  - Force Wave (3)
  - Force Heal (4)
  - Force Foresight by allowing the player to see what the enemy path will be(5)
       - Note about this is that its hard to see in the video, but a transparent droid is shown where the enemy will travel
  - Call the Force from All Jedi (6)
- Designed and implemented:
  - Health meter
  - Force meter
  - Game End screen and restart logic
  - Wired enemy alive counts into the HUD
  - Restart logic
  - Wave progression logic
  - Hit detection against enemy prefabs of the Force Powers above and lightsaber
  - Active AI movement logic of Enemies
  - Developed a PlayerStats system managing health, Force energy, regeneration, and ability costs.
- Added:
  - Lightsaber visual effects
  - Background music and audio integration
    

### Partner Contributions
- Implemented **Training Droid Mode**
- Implemented remaining Force powers:
  - Force Lightning
  - Object grabbing and throwing
- Traning Droid Enemy behaviors and laser mechanics
- Laser Sound Effect
- Deflection logic
- Laser prefab
- Light Saber Prefab
- Merging Scenes and Gamemodes

---

## Demo Video

A **2–4 minute demonstration video** is included with the submission and shows:

Link: https://youtu.be/Cbn59VJKuLs

- Gameplay from the player’s perspective
- Lightsaber combat and Force abilities
- Enemy waves and boss fight
- Real-time VR interaction using screen capture and webcam

---

## Final Notes

This project demonstrates VR interaction design, gesture-based Force abilities, real-time combat mechanics, and immersive audiovisual feedback. While some physics-related bugs remain, all required features are implemented and functional.

May the Force be with you.
