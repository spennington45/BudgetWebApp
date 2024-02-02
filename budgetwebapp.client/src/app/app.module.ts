import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NgChartsModule } from 'ng2-charts';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HomeComponent } from './components/home/home.component';
import { BudgetDetailsComponent } from './components/budget-details/budget-details.component';
import { BudgetLineItemsComponent } from './components/budget-line-items/budget-line-items.component';
import { MatTableModule } from '@angular/material/table'
import { MatExpansionModule } from '@angular/material/expansion';
import { CreateBudgetDialogComponent } from './components/create-budget-dialog/create-budget-dialog.component'
import { MatDialogModule } from '@angular/material/dialog';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatFormFieldModule } from '@angular/material/form-field';
import { FormsModule } from '@angular/forms';
import { MatNativeDateModule } from '@angular/material/core';
import { MatInputModule } from '@angular/material/input';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { MatIconModule } from '@angular/material/icon';


@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    BudgetDetailsComponent,
    BudgetLineItemsComponent,
    CreateBudgetDialogComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    NgChartsModule,
    AppRoutingModule,
    NgChartsModule,
    MatTableModule,
    MatExpansionModule,
    BrowserAnimationsModule,
    MatDialogModule,
    MatDatepickerModule,
    MatFormFieldModule,
    FormsModule,
    MatNativeDateModule,
    MatInputModule,
    MatSnackBarModule,
    MatIconModule,
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
