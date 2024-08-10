import { Component, OnInit } from '@angular/core';
import { FormBuilder,FormArray, FormGroup, Validators } from '@angular/forms';
import { UserService } from '../services/user.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  registerForm: FormGroup;

  constructor(private fb: FormBuilder, private userService: UserService) {
    this.registerForm = this.fb.group({
      username: ['', Validators.required],
      password: ['', Validators.required],
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      interests: this.fb.array([]) // Za interesovanja koristi FormArray
    });
  }

  ngOnInit(): void {
    // Ovde možeš dodati inicijalizaciju ako je potrebna
  }

  get interests() {
    return this.registerForm.get('interests') as FormArray;
  }

  addInterest(interest: string) {
    this.interests.push(this.fb.control(interest));
  }

  removeInterest(index: number) {
    this.interests.removeAt(index);
  }

  onSubmit() {
    if (this.registerForm.valid) {
      this.userService.registerUser(this.registerForm.value).subscribe(
        response => {
          console.log('User registered successfully:', response);
        },
        error => {
          console.error('Registration error:', error);
        }
      );
    }
  }
}
