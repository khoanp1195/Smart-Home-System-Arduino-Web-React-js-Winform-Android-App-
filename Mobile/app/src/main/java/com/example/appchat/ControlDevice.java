package com.example.appchat;

import android.os.Bundle;
import android.view.View;
import android.view.animation.Animation;
import android.view.animation.AnimationUtils;
import android.widget.CompoundButton;
import android.widget.ImageView;
import android.widget.Switch;
import android.widget.TextView;
import android.widget.Toast;

import androidx.annotation.NonNull;
import androidx.appcompat.app.AppCompatActivity;

import com.firebase.client.Firebase;

import com.firebase.client.FirebaseError;
import com.firebase.client.DataSnapshot;
import com.firebase.client.ValueEventListener;
import com.firebase.client.FirebaseError;
import com.google.firebase.database.DatabaseReference;

import java.util.Objects;

import static java.security.AccessController.getContext;


public class ControlDevice extends AppCompatActivity {
    TextView tvTemp, tvHumid;


    Animation rotateAnimation;
    ImageView ptAir,ptFan,ptLight;
    Switch swAir,swFan,swLight;
    boolean stateAir,stateFan,stateLight;

 //   FirebaseDatabase database = FirebaseDatabase.getInstance();
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.devicecontrol);
        anhxa();
      // final DatabaseReference mayLanh = database.getReference("MayLanh");
        //final DatabaseReference quat = database.getReference("Quat");
       // final DatabaseReference den = database.getReference("Den");
        //final DatabaseReference nhiet = database.getReference("DataUpdate1/Temperature");
        //final DatabaseReference doam = database.getReference("DataUpdate1/Humidity");




        final  Firebase mayLanh = new Firebase("https://esp8266project-8ea11-default-rtdb.firebaseio.com/Air");


        
        //thay doi database realtime
        mayLanh.addValueEventListener(new ValueEventListener() {
            @Override
            public void onDataChange(DataSnapshot dataSnapshot) {
                String value =dataSnapshot.getValue(String.class);
                if(value.equals("ON")){
                    stateAir=true; //bat thiet bi 1
                    ptAir.setImageResource(R.drawable.aircondition);
                    swAir.setChecked(true);
                    Toast.makeText(ControlDevice.this,"Bật May Lanh", Toast.LENGTH_SHORT).show();
                }else if(value.equals("OFF")){
                    stateAir=false;//tat thiet bi 11
                    swAir.setChecked(false);
                    ptAir.setImageResource(R.drawable.airconditionoff);
                    Toast.makeText(ControlDevice.this,"Tắt May lanh", Toast.LENGTH_SHORT).show();
                }
            }

            @Override
            public void onCancelled(FirebaseError firebaseError) {

            }



        });

             /*
        den.addValueEventListener(new ValueEventListener() {
            @Override
            public void onDataChange(@NonNull DataSnapshot snapshot) {
                String value =snapshot.getValue(String.class);
                if(value.equals("ON")){
                    stateLight=true;
                    ptLight.setImageResource(R.drawable.lampbulbon);
                    swLight.setChecked(true);
                    Toast.makeText(ControlDevice.this,"Bật Den", Toast.LENGTH_SHORT).show();
                }else if(value.equals("OFF")){
                    stateLight=false;
                    swLight.setChecked(false);
                    ptLight.setImageResource(R.drawable.lampbulboff);
                    Toast.makeText(ControlDevice.this,"Tắt Den", Toast.LENGTH_SHORT).show();
                }
            }
            @Override
            public void onCancelled(@NonNull DatabaseError error) {

            }
        });

        nhiet.addValueEventListener(new ValueEventListener() {
            @Override
            public void onDataChange(@NonNull DataSnapshot snapshot) {
                String value =snapshot.getValue(String.class);
                value = value +" do C";
                tvTemp.setText(value);
            }

            @Override
            public void onCancelled(@NonNull DatabaseError error) {

            }
        });
        doam.addValueEventListener(new ValueEventListener() {
            @Override
            public void onDataChange(@NonNull DataSnapshot snapshot) {
                String value =snapshot.getValue(String.class);
                value = value +" %";
                tvHumid.setText(value);
            }

            @Override
            public void onCancelled(@NonNull DatabaseError error) {

            }
        });

        quat.addValueEventListener(new ValueEventListener() {
            @Override
            public void onDataChange(@NonNull DataSnapshot snapshot) {
                String value =snapshot.getValue(String.class);
                if(value.equals("ON")){
                    stateFan=true;
                    rotateAnimation();
                    swFan.setChecked(true);
                    Toast.makeText(ControlDevice.this,"Bật Quat", Toast.LENGTH_SHORT).show();
                }else if(value.equals("OFF")){
                    stateFan=false;
                    swFan.setChecked(false);
                    ptFan.clearAnimation();
                    Toast.makeText(ControlDevice.this,"Tắt Quat", Toast.LENGTH_SHORT).show();
                }
            }
            @Override
            public void onCancelled(@NonNull DatabaseError error) {

            }
        });
        ptAir.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                stateAir =!stateAir;
                if(stateAir){
                    mayLanh.setValue("ON");
                    ptAir.setImageResource(R.drawable.aircondition);
                    swAir.setChecked(true);
                }else{
                    mayLanh.setValue("OFF");
                    ptAir.setImageResource(R.drawable.airconditionoff);
                    swAir.setChecked(false);
                }
            }
        });
        ptLight.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                stateLight =!stateLight;
                if(stateLight){
                    den.setValue("ON");
                    ptLight.setImageResource(R.drawable.lampbulbon);
                    swLight.setChecked(true);
                }else{
                    den.setValue("OFF");
                    ptLight.setImageResource(R.drawable.lampbulboff);
                    swLight.setChecked(false);
                }
            }
        });
        ptFan.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                stateFan =!stateFan;
                if(stateFan){
                    quat.setValue("ON");
                    rotateAnimation();
                    swFan.setChecked(true);
                }else{
                    quat.setValue("OFF");
                    ptFan.clearAnimation();
                    swFan.setChecked(false);
                }
            }
        });
        swAir.setOnCheckedChangeListener(new CompoundButton.OnCheckedChangeListener() {
            @Override
            public void onCheckedChanged(CompoundButton buttonView, boolean isChecked) {
                if(isChecked){
                    mayLanh.setValue("ON");
                }else{
                    mayLanh.setValue("OFF");
                }
            }
        });
        swLight.setOnCheckedChangeListener(new CompoundButton.OnCheckedChangeListener() {
            @Override
            public void onCheckedChanged(CompoundButton buttonView, boolean isChecked) {
                if(isChecked){
                    den.setValue("ON");
                }else{
                    den.setValue("OFF");
                }
            }
        });
        swFan.setOnCheckedChangeListener(new CompoundButton.OnCheckedChangeListener() {
            @Override
            public void onCheckedChanged(CompoundButton buttonView, boolean isChecked) {
                if(isChecked){
                    quat.setValue("ON");
                }else{
                    quat.setValue("OFF");
                }
            }
        });

         */

    }

    private void anhxa() {
        ptAir = findViewById(R.id.ptAir);
        ptLight = findViewById(R.id.ptLight);
        ptFan = findViewById(R.id.ptCanhQuat);
        swAir = findViewById(R.id.swAir);
        swFan = findViewById(R.id.swFan);
        swLight = findViewById(R.id.swLight);

        tvTemp = findViewById(R.id.tvTemp);
        tvHumid = findViewById(R.id.tvHumid);
    }
    private void rotateAnimation() {
        rotateAnimation = AnimationUtils.loadAnimation(this,R.anim.rotationfan);
        ptFan.startAnimation(rotateAnimation);
    }
}
