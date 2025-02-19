 ## Overview
 **Time spent: ~30 hours.**
 **Unity version: 2021.3.38f1**
 
 This is a case study assignment done as a recreation of Match Factory. There are various systems that I feel I
 have over-engineered to some extent, but I preferred this approach for a code assignment over
 quick and dirty prototype code that is more appropriate for the short development time.

 ### Packages used
 * DoTween
 * SceneReference: Small editor utility for convenient references.
 * Vibration package: More control on device haptics.
 * GenericEvent Bus: Send and forget messages to help decoupling.
 * Free 2D and 3D asset packs from the Unity Asset Store and Kenney assets.


https://github.com/user-attachments/assets/fc19307f-de2f-46f4-ac07-c979070370a7


 ## About the project
 * A simple twist on the gameplay where upon filling all slots, the game will empty the row,
 but spawn more items to clear.
 * Lightly implemented addressables to make use of loading prefabs from a folder in Unity,
 which makes it easier to add and remove new object types without a scriptable object.
 * There’s a simple version of a Window system that uses timelines as a way of
 showing/hiding windows. This is useful to add a large degree of control and freedom in
 the editor, which an artist or designer can extensively change.
 * Levels use the same scene and are initialized with goals and themes through scriptable
 object LevelConfigs.
 * The main code areas that control gameplay are the InteractablesManager, MatchRow,
 and VisualMatcher. End game windows are triggered from GoalsManager.

 ## What could have gone better
 I’ve mentioned over-engineering at the start of this document. My hope is the current code and
 features transmit an idea of my vision for setup on a game with a longer lifetime. However it
 stands that some of it is unnecessary, and looking back, I would have wanted to spend more
 time on the core gameplay itself.
 Likewise the twist to the core gameplay only came together near the end. I’ve had an idea of
 what I wanted to do from the beginning, but I had to settle for something a bit simpler due to
 placing focus on other areas.

 ## Potential next steps
 Given more work time, these are some of the features to start with, as per assignment suggestions:
 1. More polish with animations and effects when making matches.
 2. Adding a mockup store/leaderboard
