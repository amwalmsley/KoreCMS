import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Customer } from '../models/customer.model';
import { environment } from '../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class CustomersService {
  constructor(private http: HttpClient) { }

  getAll(pageNumber: number, pageSize: number): Observable<{ totalCount: number, items: Customer[] }> {
    return this.http.get<{ totalCount: number, items: Customer[] }>(`${environment.apiHost}/Customers?pageSize=${pageSize}&pageNumber=${pageNumber}`);
  }

  get(id: number): Observable<Customer> {
    return this.http.get<Customer>(`${environment.apiHost}/Customers/${id}`);
  }

  create(customer: Customer) {
    customer.id = 0;
    console.log('attepting to create', customer);
    return this.http.post(`${environment.apiHost}/Customers`, customer);
  }

  update(customer: Customer) {
    return this.http.put(`${environment.apiHost}/Customers`, customer);
  }

  delete(id: number) {
    return this.http.delete(`${environment.apiHost}/Customers/${id}`);
  }
}
