package com.example.appchat;


import android.content.Intent;
import android.os.Bundle;
import android.view.View;
import android.widget.Button;
import android.widget.ImageView;
import android.widget.ListView;

import androidx.appcompat.app.AppCompatActivity;

import com.google.firebase.database.DatabaseReference;
import com.google.firebase.database.FirebaseDatabase;


public class controllight extends AppCompatActivity {

    ImageView imageView1;
    ImageView imageView2;
    ImageView imageView3;
    ImageView imageView4;
    ListView _dynamic;
    Button button1,button2;
    ImageView back;




    boolean stateToFirebase1;
    boolean stateToFirebase2;
    boolean stateToFirebase3;
    boolean stateToFirebase4;

    FirebaseDatabase database = FirebaseDatabase.getInstance();
    DatabaseReference myRef = database.getReference("devices");
    DatabaseReference FirebaseLight1 = myRef.child("light1");
    DatabaseReference FirebaseLight2 = myRef.child("light2");
    DatabaseReference FirebaseLight3 = myRef.child("light3");
    DatabaseReference FirebaseLight4 = myRef.child("light4");
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.conlight);
        imageView1 = (ImageView) findViewById(R.id.img1);
        imageView2 = (ImageView) findViewById(R.id.img2);
        imageView3 = (ImageView) findViewById(R.id.img3);
        imageView4 = (ImageView) findViewById(R.id.img4);

        back= findViewById(R.id.back);
        back.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                Intent intent = new Intent(controllight.this, layout1.class);
                startActivity(intent);
            }
        });


    }



    public void imgLight1(View view) {
        stateToFirebase1 = !stateToFirebase1;
        FirebaseLight1.setValue(stateToFirebase1);
        if(stateToFirebase1)
            imageView1.setImageResource(R.drawable.lighton);
        else
            imageView1.setImageResource(R.drawable.lightoff);
    }

    public void imgLight2(View view) {
        stateToFirebase2 = !stateToFirebase2;
        FirebaseLight2.setValue(stateToFirebase2);
        if(stateToFirebase2)
            imageView2.setImageResource(R.drawable.lighton);
        else
            imageView2.setImageResource(R.drawable.lightoff);
    }

    public void imgLight3(View view) {
        stateToFirebase3 = !stateToFirebase3;
        FirebaseLight3.setValue(stateToFirebase3);
        if(stateToFirebase3)
            imageView3.setImageResource(R.drawable.lighton);
        else
            imageView3.setImageResource(R.drawable.lightoff);
    }

    public void imgLight4(View view) {
        stateToFirebase4 = !stateToFirebase4;
        FirebaseLight4.setValue(stateToFirebase4);
        if(stateToFirebase4)
            imageView4.setImageResource(R.drawable.lighton);
        else
            imageView4.setImageResource(R.drawable.lightoff);
    }
}