package com.example.appchat.ChatBot;

public class ChatsModal {
    private  String message;
    private String sender;

    public ChatsModal(String message, String sender) {
        this.message = message;
        this.sender = sender;

    }

    public String getMessage() {
        return message;
    }

    public ChatsModal setMessage(String message) {
        this.message = message;
        return this;
    }

    public String getSender() {
        return sender;
    }

    public ChatsModal setSender(String sender) {
        this.sender = sender;
        return this;
    }
}
