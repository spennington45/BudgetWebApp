import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { BudgetLineItemService } from '../../services/budget-line-item.service';
import { BudgetLineItems, Category, SourceType } from '../../models';
import { CategoryService } from '../../services/category.service';
import { SourceTypeService } from '../../services/source-type.service';
import { AddLookupDialogComponent } from '../add-lookup-dialog/add-lookup-dialog.component';
import { MatDialog } from '@angular/material/dialog';

@Component({
  selector: 'app-transaction-details',
  templateUrl: './transaction-details.component.html',
  styleUrls: ['./transaction-details.component.scss']
})
export class TransactionDetailsComponent implements OnInit {

  budgetId!: number;
  transactionId!: number;
  year!: number;
  month!: number;

  categories: Category[] = [];
  sourceTypes: SourceType[] = [];
  categoryFilter = '';
  sourceTypeFilter = '';

  loading = true;
  editing = false;

  transaction!: BudgetLineItems | null;
  editModel!: BudgetLineItems;

  constructor(
    private route: ActivatedRoute,
    private budgetService: BudgetLineItemService,
    private router: Router,
    private categoryService: CategoryService,
    private sourceTypeService: SourceTypeService,
    private dialog: MatDialog
  ) { }

  ngOnInit(): void {
    this.budgetId = Number(this.route.snapshot.paramMap.get('id'));
    this.transactionId = Number(this.route.snapshot.paramMap.get('transactionId'));
    this.year = Number(this.route.snapshot.paramMap.get('year'));
    this.month = Number(this.route.snapshot.paramMap.get('month'));

    this.loadTransaction();
    this.getSourceTypes();
    this.getCategories();
  }

  loadTransaction() {
    this.loading = true;

    this.budgetService.getBudgetLineItemById(this.transactionId).subscribe({
      next: (item) => {
        this.transaction = item;
        this.editModel = JSON.parse(JSON.stringify(item));
        this.loading = false;
        this.syncSelectedCategory();
        this.syncSelectedSourceType();
      },
      error: () => {
        this.transaction = null;
        this.loading = false;
      }
    });
  }

  startEdit() {
    if (!this.transaction) return;
    this.editing = true;
    this.editModel = JSON.parse(JSON.stringify(this.transaction));
    this.syncSelectedCategory();
    this.syncSelectedSourceType();
  }

  cancelEdit() {
    this.editing = false;
  }

  saveEdit() {
    this.budgetService.updateBudgetLineItem(this.editModel).subscribe({
      next: () => {
        this.loadTransaction();
        this.editing = false;
      }
    });
  }

  goBack() {
    this.router.navigate(['/budget', this.year, this.month, this.budgetId]);
  }

  getSourceTypes() {
    this.sourceTypeService.getAllSourceTypes().subscribe(response => {
      this.sourceTypes = response;
      this.syncSelectedSourceType();
    });
  }

  getCategories() {
    this.categoryService.getAllCategories().subscribe(response => {
      this.categories = response;
      this.syncSelectedCategory();
    });
  }

  syncSelectedCategory() {
    if (!this.categories.length || !this.transaction) return;

    const match = this.categories.find(
      c => c.categoryId === this.transaction!.category.categoryId
    );

    if (match) {
      this.editModel.category = match;
      this.editModel.categoryId = match.categoryId;
    }
  }

  syncSelectedSourceType() {
    if (!this.sourceTypes.length || !this.transaction) return;

    const match = this.sourceTypes.find(
      s => s.sourceTypeId === this.transaction!.sourceType.sourceTypeId
    );

    if (match) {
      this.editModel.sourceType = match;
      this.editModel.sourceTypeId = match.sourceTypeId;
    }
  }

  openAddCategoryDialog() {
    const dialogRef = this.dialog.open(AddLookupDialogComponent, {
      width: '400px',
      data: {
        title: 'Add Category',
        fieldLabel: 'Category Name',
        existing: this.categories,
        property: 'categoryName',
        saveFn: (payload: any) => this.categoryService.addCategory(payload)
      }
    });

    dialogRef.afterClosed().subscribe((newCategory: Category) => {
      if (newCategory) {
        this.categories.push(newCategory);
        this.editModel.category = newCategory;
        this.editModel.categoryId = newCategory.categoryId;
      }
    });
  }

  openAddSourceTypeDialog() {
    const dialogRef = this.dialog.open(AddLookupDialogComponent, {
      width: '400px',
      data: {
        title: 'Add Source Type',
        fieldLabel: 'Source Type Name',
        existing: this.sourceTypes,
        property: 'sourceName',
        saveFn: (payload: any) => this.sourceTypeService.addSourceType(payload)
      }
    });

    dialogRef.afterClosed().subscribe((newType: SourceType) => {
      if (newType) {
        this.sourceTypes.push(newType);
        this.editModel.sourceType = newType;
        this.editModel.sourceTypeId = newType.sourceTypeId;
      }
    });
  }

  get filteredCategories() {
    return this.categories.filter(c =>
      c.categoryName.toLowerCase().includes(this.categoryFilter.toLowerCase())
    );
  }

  get filteredSourceTypes() {
    return this.sourceTypes.filter(s =>
      s.sourceName.toLowerCase().includes(this.sourceTypeFilter.toLowerCase())
    );
  }

  onCategoryChange(category: Category) {
    this.editModel.category = category;
    this.editModel.categoryId = category.categoryId;
  }

  onSourceTypeChange(sourceType: SourceType) {
    this.editModel.sourceType = sourceType;
    this.editModel.sourceTypeId = sourceType.sourceTypeId;
  }
}