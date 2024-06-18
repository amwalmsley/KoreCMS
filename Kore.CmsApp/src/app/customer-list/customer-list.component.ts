import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { Customer } from '../models/customer.model';
import { CustomersService } from '../services/customers.service';

@Component({
  selector: 'app-customer-list',
  templateUrl: './customer-list.component.html',
  styleUrl: './customer-list.component.css'
})
export class CustomerListComponent implements AfterViewInit, OnInit {
  displayedColumns = ['id', 'lastName', 'firstName', 'middleInitial', 'title', 'email', 'phone'];
  dataSource: MatTableDataSource<Customer> = new MatTableDataSource();
  isLoading = false;
  pageSize = 10;
  currentPage = 0;
  totalRows = 0;
  pageSizeOptions: number[] = [5, 10, 25, 100];

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  
  constructor(
    private router: Router,
    private customersService: CustomersService
  ) {}

  ngOnInit() {
    this.loadData();
  }

  ngAfterViewInit(): void {
    this.dataSource.paginator = this.paginator;
  }

  onRowClick(id: number) {
    this.router.navigate(['/customer', id]);
  }

  loadData() {
    this.isLoading = true;
    this.customersService.getAll(this.currentPage, this.pageSize)
      .subscribe(result => {
        this.totalRows = result.totalCount;
        this.dataSource.data = result.items;
        setTimeout(() => {
          if (this.paginator) {
            this.paginator.pageIndex = this.currentPage;
            this.paginator.length = result.totalCount;
          }
        });
        this.isLoading = false;
      });
  }

  pageChanged(event: PageEvent) {
    this.pageSize = event.pageSize;
    this.currentPage = event.pageIndex;
    this.loadData();
  }
}
