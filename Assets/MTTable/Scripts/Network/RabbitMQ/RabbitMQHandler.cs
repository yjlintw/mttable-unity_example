using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


using RabbitMQ.Client;
using RabbitMQ.Client.Events;


public class RabbitMQHandler : MonoBehaviour {

	// Client client; 

	// CustomSocketFactory cf; 
	ConnectionFactory cf; 

	IModel channel; 

    EventingBasicConsumer consumer; 

    MessageAdaptor msgAdaptor = new MessageAdaptor();


    private Dictionary<string, int> tokenIdMap = new Dictionary<string, int> {
        {"368A", 6},
        {"897E", 2},
        {"03E9", 5}
    };

    public string currentMidAirId = "";
    public int currentTableTagId = -1;
    public int currentTokenDist = 10000000;
    public int DIST_THRESHOLD = 30;

    public TrackerHub trackerHub;

	// Use this for initialization
	void Start () {

		Debug.Log ("RabbitMQ Handler Start ... ");

        msgAdaptor.subscribe(MessageAdaptor.EventType.Speech, 
            this.onSpeech);

        msgAdaptor.subscribe(MessageAdaptor.EventType.MidAirTokens, 
            this.onMidAirToken);

        msgAdaptor.subscribe(MessageAdaptor.EventType.Hotword,
            this.onHotword);
        
    }

    void Update() {
        if (Input.GetKeyDown (KeyCode.S)) {
            trackerHub.setActiveMarker(0);
        } else if (Input.GetKeyDown(KeyCode.D)) {
            trackerHub.removeActiveMarker();
        } else if (Input.GetKeyDown(KeyCode.F)) {
            // trackerHub.activeMarker.setAudio("Lion");
        }
    }

    private void onSpeech (MessageAdaptor.EventMessage message) {
        
        var speechMessage = message as MessageAdaptor.SpeechMessage;
        
        Debug.Log (speechMessage.intent);
        trackerHub.setHotword(false);
        if (trackerHub.activeMarker && speechMessage.intent == "PlaySound") {
            // trackerHub.activeMarker.setAudio(speechMessage.entity);
        }
        
        // if(args.RoutingKey
    }
    
    private void onHotword(MessageAdaptor.EventMessage message) {
        var hotwordMessage = message as MessageAdaptor.HotwordMessage;

        Debug.Log(hotwordMessage.hotword);
        trackerHub.setHotword(true);

    }	
    private void onMidAirToken(MessageAdaptor.EventMessage message) {
        var midAirTMessage = message as MessageAdaptor.MidAirTokenMessage;
        // Debug.Log (midAirTMessage.fromTokenId);
        // Debug.Log (midAirTMessage.toTokenId);
        // Debug.Log (midAirTMessage.distance);
        string newMidAirId = midAirTMessage.toTokenId;
        int newTableTagId = tokenIdMap[newMidAirId];
        int dist = midAirTMessage.distance;
        if ( dist < DIST_THRESHOLD ) {
            if (trackerHub.activeMarker == null || dist < currentTokenDist) {
                currentMidAirId = newMidAirId;
                currentTableTagId = newTableTagId;
                currentTokenDist = dist;
                trackerHub.setActiveMarker(currentTableTagId);
            }
        } else if (currentTableTagId == newTableTagId && dist > DIST_THRESHOLD) {
            currentMidAirId = "";
            currentTableTagId = -1;
            currentTokenDist = 10000000;
            trackerHub.removeActiveMarker();
        }

    }

	

}
