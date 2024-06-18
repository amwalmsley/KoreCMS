import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { CustomersService } from '../services/customers.service';

@Component({
  selector: 'app-customer',
  templateUrl: './customer.component.html',
  styleUrl: './customer.component.css'
})
export class CustomerComponent implements OnInit {
  form!: FormGroup;
  id!: number;
  isAddMode!: boolean;
  isLoading!: boolean;

  constructor(
    private formBuilder: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private customersService: CustomersService
  ) { }

  ngOnInit() {
    this.id = parseInt(this.route.snapshot.params['id']);
    this.isAddMode = !this.id;

    this.form = this.formBuilder.group({
      id: [''],
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      middleInitial: ['', Validators.maxLength(1)],
      title: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      phone: ['', [Validators.required, Validators.pattern('')]]
    });

    if (this.id) {
      this.isLoading = true;
      this.customersService.get(this.id)
        .subscribe(customer => {
          this.form.patchValue(customer);
          this.isLoading = false;
        });
    }
  }

  onSubmit() {
    if (this.form.invalid) {
      return;
    }

    if (this.isAddMode) {
      this.customersService.create(this.form.value)
        .subscribe(_ => {
          this.router.navigate(['/customers']);
        });

    } else {
      this.customersService.update(this.form.value)
        .subscribe(_ => {
          this.router.navigate(['/customers']);
        });
    }
  }

  onDelete() {
    this.customersService.delete(this.id)
      .subscribe(_ => {
        this.router.navigate(['/customers']);
      });
  }
}
