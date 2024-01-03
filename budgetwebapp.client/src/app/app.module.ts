import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { ChartsModule } from 'ng2-charts';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HomeComponent } from './components/home/home.component';
import { BudgetDetailsComponent } from './components/budget-details/budget-details.component';
import { BudgetLineItemsComponent } from './components/budget-line-items/budget-line-items.component';

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    BudgetDetailsComponent,
    BudgetLineItemsComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    ChartsModule,
    AppRoutingModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
