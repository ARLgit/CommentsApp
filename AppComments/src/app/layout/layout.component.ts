import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';
import { MaterialsModule } from '../MaterialsModule/materials/materials.module';

@Component({
  selector: 'app-layout',
  standalone: true,
  imports: [CommonModule, MaterialsModule, RouterOutlet, RouterLink, RouterLinkActive, FormsModule],
  templateUrl: './layout.component.html',
  styleUrl: './layout.component.css'
})
export class LayoutComponent implements OnInit {
  
  ngOnInit() {
  }

}
