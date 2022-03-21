package com.example.appchat.ChatBot;

public class MsgModal {
    private String cnt;

    public MsgModal(String cnt) {
        this.cnt = cnt;
    }

    public String getCnt() {
        return cnt;
    }

    public MsgModal setCnt(String cnt) {
        this.cnt = cnt;
        return this;
    }
}
