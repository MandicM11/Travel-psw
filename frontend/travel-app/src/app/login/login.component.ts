import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { UserService } from '../services/user.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  loginForm: FormGroup;
  errorMessage: string = '';

  constructor(private fb: FormBuilder, private userService: UserService, private router: Router) {
    this.loginForm = this.fb.group({
      username: ['', Validators.required],
      password: ['', Validators.required]
    });
  }

  ngOnInit(): void {
    this.loginForm = this.fb.group({
      username: ['', Validators.required],
      password: ['', Validators.required]
    });
  }

  onSubmit() {
    if (this.loginForm.valid) {
      const formData = this.loginForm.value;
      this.userService.loginUser(formData).subscribe(
        response => {
          localStorage.setItem('token', response.token); // Čuvanje tokena u lokalnoj memoriji
          const userId = this.userService.getUserIdFromToken();
          if (userId) {
            // Preuzimanje postojeće korpe, jer je korpa već kreirana prilikom registracije
            this.userService.getCartByUserId(userId).subscribe(carts => {
              if (carts.length > 0) {
                localStorage.setItem('cartId', carts[0].id); // Postavi ID korpe u lokalnu memoriju
              }
              this.router.navigate(['/']); // Redirektuj na glavnu stranicu ili drugu nakon uspešnog login-a
            });
          }
        },
        error => {
          this.errorMessage = 'Invalid username or password'; // Prikaz poruke o grešci
          console.error('Login error:', error);
        }
      );
    }
  }
}
