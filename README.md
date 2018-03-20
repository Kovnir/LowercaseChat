# Lowercase Chat (test assignment)
## Chat, that use lowercase characters only.

![login](/readmeimg/1.png)
![chatroom selection](/readmeimg/2.png)
![chat](/readmeimg/3.png)

Made with [**Unity**](https://unity3d.com) and [**GameSparks Framework**](https://gamesparks.com/)

It was made as test task in employment.

Chat messages are sent to all users in current challenge.
To run source code you must create **challenge "Chat"** (name and short code must be "Chat") and several events on server side.

## Events
### ChatMessage - sends message to all users in current challenge.

Attributes:
* challengeId - id of this challenge (chat room);
* message - sending message.

Cloud Code:
```javascript
var challengeId = Spark.getData().challengeId;
var players = Spark.getChallenge(challengeId).getAcceptedPlayerIds();
var message = Spark.getData().message;
var dName = Spark.getPlayer().getDisplayName();
var json = {"displayName":dName, "message":message};
//Send the message to all the players in the list
Spark.sendMessageById(json, players);
```


### GetChatUsers - gets list of username of all users in current chat (Challenge).

Attributes:
* ChallengeId - id of this challenge (chat room).

Cloud Code:
```javascript
var challengeId = Spark.getData().ChallengeId;
var chatUsers = Spark.getChallenge(challengeId).getAcceptedPlayerIds();
var allPlayerList = Spark.runtimeCollection("playerList");
var result = [];
for (var i = 0; i < chatUsers.length; i++) 
{
    result.push(allPlayerList.findOne({ "id" : chatUsers[i] }).username); 
}
var json = {"username":result};
Spark.setScriptData("users", json);
```


### GetUserId - gets userId by username.
Attributes:
* username - name of user.

Cloud Code:
```javascript
var username = Spark.getData().username;
var playerData = Spark.runtimeCollection("playerList").findOne({ "username" : username });
Spark.setScriptData("id", playerData.id);
```

### LeaveChat - leaves chat room.
Attributes 
ChallengeId - id of this challenge (chat room).

Cloud Code:
```javascript
var username = Spark.getData().username;
var playerData = Spark.runtimeCollection("playerList").findOne({ "username" : username });
Spark.setScriptData("id", playerData.id);
```

Also you must override event RegistrationResponse to save names and IDs conformity.

Cloud Code:
```javascript
var playerID = Spark.getData().userId;
var displayName = Spark.getData().displayName;
if(Spark.getData().newPlayer) // only save if it is a new player
{
    Spark.runtimeCollection("playerList").insert({
        "id" : playerID,
        "username" : Spark.getPlayer().getUserName()
    })
}
```
