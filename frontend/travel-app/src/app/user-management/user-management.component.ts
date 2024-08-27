import { Component, OnInit } from '@angular/core';
import { UserService } from '../services/user.service'; 
import { Observable } from 'rxjs';

@Component({
  selector: 'app-user-management',
  templateUrl: './user-management.component.html',
  styleUrls: ['./user-management.component.css']
})
export class UserManagementComponent implements OnInit {
  users: any[] = []; // Definišite tip prema vašim podacima

  constructor(private userService: UserService) {}

  ngOnInit(): void {
    this.loadUsers();
  }

  loadUsers(): void {
    this.userService.getUsers().subscribe({
      next: (data) => {
        this.users = data;
      },
      error: (err) => {
        console.error('Error loading users:', err);
      }
    });
  }

  blockUser(userId: number): void {
    this.userService.blockUser(userId).subscribe({
      next: () => {
        this.loadUsers(); // Reload users after blocking
      },
      error: (err) => {
        console.error('Error blocking user:', err);
      }
    });
  }

  unblockUser(userId: number): void {
    this.userService.unblockUser(userId).subscribe({
      next: () => {
        this.loadUsers(); // Reload users after unblocking
      },
      error: (err) => {
        console.error('Error unblocking user:', err);
      }
    });
  }
}
