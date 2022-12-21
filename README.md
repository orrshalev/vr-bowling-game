# VR Bowling Game
Dr. Kyle Johnsen's Virtual Reality Final Project

Group 1

Team Members: Tyler Patzer, Orr Shalev, Blake Strauss, Razvan Beldeanu

![mspaint_qn2SQILdsD](https://user-images.githubusercontent.com/32816567/206780585-20b4a425-263d-46c2-aaec-eac7b306d20a.png)

# Description
Test out your bowling skills in our 1-on-1 bowling match! Players will be able to select a room, choose their avatar, and bowl with a friend. Player who knocks the most pins wins! Complete with networking, game mechanics, NPC avatars, sound, and more.

# Problems Encountered
Tyler Patzer: It was very difficult to get networking to work properly. We spent a lot of time testing and debugging code to get actions synced across multiple devices and to properly switch ownership of objects in the scene when necessary. In particular, we had a problem where the TakeOwnership() function would be called on both clients, which caused one player to receieve ownership for a split second, but then would immediately lose ownership. We fixed this problem by making sure TakeOwnernship() would only be called on the client who does not have ownership, after the previous owner had finished their turn bowling.

Orr Shalev: The rooms list was not always correct when refreshing, I was able to get around it for all cases except for when a room has already been joined by the player, which didn’t matter for the current user experience but would need to be fixed in a more full-sized game. There were some limitations with the XR Interaction toolkit which if expanded upon would have likely resulted in a more accurate throwing mechanism.

Blake Strauss: The biggest difficulty was figuring out how to manage each player's score while also switching turns of each player. After looking at other group's projects, games where you play against other clients are harder to network compared to games where you cooperatively work with other clients (less objects to network and sync). Also, there are many different ways to check for how the pins should be counted as "knocked down" in the game logic. There are also many edge cases to account for, such as the ball getting stuck, players falling out the map, etc that we did not discover until others playtested our game during the presentation. This further reinforces how important play testing is in the game development cycle.

Razvan Beldeanu: Networking was definitely the hardest part of the entire project, and the biggest problem I faced was getting the game logic to sync across both clients. Originally both clients would keep track of both scores and when one player finished their turns it would issue a switchTurn command which changed ownership, this did not work and the ownership never changed. After hours of debugging I figured out the real problem. The ownership was changing but since both clients kept track of both scores they would both hit switchTurn at the same time which changed ownership then changed it back. To fix this I rewrote the logic so that each client only took control of their score and turn which worked wonderfully. 

# Video Demo
[![hyperlink to demo video](https://img.youtube.com/vi/Hwz4vMF3uUQ/0.jpg)](https://www.youtube.com/watch?v=Hwz4vMF3uUQ)

Click the picture above to open the YouTube video demonstration.
