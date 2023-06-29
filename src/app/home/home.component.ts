import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {

  registerMode = false;
  users: any;
  
  constructor() { }

  ngOnInit(): void {
  }

  //show / hide form
  registerToggle(){
    this.registerMode = !this.registerMode;
  }


  //Cancel registration
  cancelRegistraterMode(event: boolean){
    this.registerMode = event;
  }
}
