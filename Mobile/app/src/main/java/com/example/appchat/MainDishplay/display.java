package com.example.appchat.MainDishplay;

import android.content.Intent;
import android.os.Bundle;
import android.view.View;
import android.widget.ImageView;
import android.widget.TextView;

import androidx.appcompat.app.AppCompatActivity;

import com.example.appchat.Login1.Mainactivity;
import com.example.appchat.R;
import com.example.appchat.controllight;
import com.firebase.client.DataSnapshot;
import com.firebase.client.Firebase;
import com.firebase.client.FirebaseError;
import com.firebase.client.ValueEventListener;
import com.google.firebase.database.FirebaseDatabase;

import java.util.Objects;

import az.plainpie.PieView;
import az.plainpie.animation.PieAngleAnimation;
import pl.pawelkleczkowski.customgauge.CustomGauge;


public class display extends AppCompatActivity {
/*

    DatabaseReference myRef = database.getReference("devices");
    DatabaseReference FirebaseLight1 = myRef.child("light1");
 */
    private PieView pieView_hum;
    private PieView pieView_soil;
    private CustomGauge customGauge;
    private TextView textView;
    ImageView imageView1,back,chart,light;
   boolean stateToFirebase1;
   FirebaseDatabase database = FirebaseDatabase.getInstance();



    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.pair);
        chart = findViewById(R.id.chart);
        chart.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                Intent  intent = new Intent(display.this, com.example.appchat.chart2.class);
                startActivity(intent);
            }
        });
        light = findViewById(R.id.light);
        light.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                Intent  intent = new Intent(display.this, controllight.class);
                startActivity(intent);
            }
        });
            back = findViewById(R.id.back);
            back.setOnClickListener(new View.OnClickListener() {
                @Override
                public void onClick(View v) {
                    Intent  intent = new Intent(display.this, Mainactivity.class);
                    startActivity(intent);
                }
            });

 //  imageView1 = (ImageView) findViewById(R.id.img1);
     customGauge = findViewById(R.id.gauge2);
     textView = findViewById(R.id.text1);
    pieView_hum = findViewById(R.id.pieView_hum);
     //pieView_soil = findViewById(R.id.pieView_soil);
      // imageView = findViewById(R.id.image);


        Firebase.setAndroidContext(Objects.requireNonNull(pieView_hum.getContext()));
   //     Firebase mRef3 = new Firebase("https://trac-nghiem-2-20ffc.firebaseio.com/DataUpdate1/Humidity");
        Firebase mRef4 = new Firebase("https://esp8266project-8ea11-default-rtdb.firebaseio.com/RealtimeData/Humidity");
       final Firebase mRef1 = new Firebase("https://esp8266project-8ea11-default-rtdb.firebaseio.com/RealtimeData/Temperature");
   /*     Firebase mRef2 = new Firebase("https://trac-nghiem-2-20ffc.firebaseio.com/devices/light1");

        Firebase mRef4 = new Firebase("https://iot-phong-cn.firebaseio.com/Smart_Farm1/sensor/dht/hum");


        Firebase btnfan1 = new Firebase("https://iot-phong-cn.firebaseio.com/Smart_Farm1/Status/Fan");
        Firebase btnlamp1 = new Firebase("https://trac-nghiem-2-20ffc.firebaseio.com/devices/light1");
        Firebase btnpump1 = new Firebase("https://iot-phong-cn.firebaseio.com/Smart_Farm1/Status/Pump");
        Firebase btnmis1 = new Firebase("https://iot-phong-cn.firebaseio.com/Smart_Farm1/Status/Mis");
        Firebase btnfan21 = new Firebase("https://iot-phong-cn.firebaseio.com/Smart_Farm1/Status/Fan2");

    */

/*
        pieView_hum.setPercentageBackgroundColor(getResources().getColor(R.color.bluehigh));
//        pieView_hum.setMainBackgroundColor(getResources().getColor(R.color.white));


        pieView_soil.setPercentageBackgroundColor(getResources().getColor(R.color.browhigh));
//        pieView_soil.setMainBackgroundColor(getResources().getColor(R.color.white));

 */


        mRef1.addValueEventListener(new ValueEventListener() {
            @Override
            public void onDataChange(DataSnapshot dataSnapshot) {
                String value = dataSnapshot.getValue(String.class);
                textView.setText(value + "°C");
                int i = Integer.parseInt(value.replaceAll("[\\D]", ""));
                customGauge.setValue(i * 10);
            }

            @Override
            public void onCancelled(FirebaseError firebaseError) {

            }
        });
        mRef4.addValueEventListener(new ValueEventListener() {
            @Override
            public void onDataChange(DataSnapshot dataSnapshot) {
                String value = dataSnapshot.getValue(String.class);

                float i = Float.parseFloat(value);
                pieView_hum.setPercentage(i);
                pieView_hum.setPieInnerPadding(50);
                pieView_hum.setPercentageTextSize(30);
                PieAngleAnimation animation = new PieAngleAnimation(pieView_hum);
                animation.setDuration(3000); //This is the duration of the animation in millis
                pieView_hum.startAnimation(animation);
            }

            @Override
            public void onCancelled(FirebaseError firebaseError) {

            }
        });
/*
        mRef4.addValueEventListener(new ValueEventListener() {
            @Override
            public void onDataChange(DataSnapshot dataSnapshot) {
                String value = dataSnapshot.getValue(String.class);

                float i = Float.parseFloat(value);
                pieView_hum.setPercentage(i);
                pieView_hum.setPieInnerPadding(30);
                pieView_hum.setPercentageTextSize(20);
                PieAngleAnimation animation = new PieAngleAnimation(pieView_hum);
                animation.setDuration(2000); //This is the duration of the animation in millis
                pieView_hum.startAnimation(animation);
            }

            @Override
            public void onCancelled(FirebaseError firebaseError) {

            }
        });

        mRef3.addValueEventListener(new ValueEventListener() {
            @Override
            public void onDataChange(DataSnapshot dataSnapshot) {
                String value = dataSnapshot.getValue(String.class);
                float i = Float.parseFloat(value);
                pieView_soil.setPieInnerPadding(30);
                pieView_soil.setPercentageTextSize(20);
                pieView_soil.setPercentage(i);
                PieAngleAnimation animation = new PieAngleAnimation(pieView_soil);
                animation.setDuration(2000); //This is the duration of the animation in millis
                pieView_soil.startAnimation(animation);
            }

            @Override
            public void onCancelled(FirebaseError firebaseError) {

            }
        });

    }

 */
    }
    /*
    public void imgLight1(View view) {
        stateToFirebase1 = !stateToFirebase1;
        FirebaseLight1.setValue(stateToFirebase1);
        if(stateToFirebase1)
            imageView1.setImageResource(R.drawable.lighton);
        else
            imageView1.setImageResource(R.drawable.lightoff);
    }

     */
}
