import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  //send method to parent component
  @Output() cancelRegister = new EventEmitter();
  model: any = {}

  constructor(private accountService: AccountService,private toastr: ToastrService) { }

  ngOnInit(): void {
  }

  //register
  register(){
    this.accountService.regiter(this.model).subscribe({
      next: () => {
        this.cancel();
      },
      error: error => {
        this.toastr.error(error.error);
        console.log(error);
        
      }
    })
  }
  //cancel
  cancel(){
    this.cancelRegister.emit(false);
  }

}
