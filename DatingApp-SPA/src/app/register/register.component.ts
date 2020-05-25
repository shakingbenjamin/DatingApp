import {
  Component,
  OnInit,
  Output,
  EventEmitter,
  ChangeDetectionStrategy,
} from '@angular/core';
import { AuthService } from '../_services/auth.service';
import { AlertifyService } from '../_services/alertify.service';
import {
  FormGroup,
  FormControl,
  Validators,
  FormBuilder,
} from '@angular/forms';
import { BsDatepickerConfig } from 'ngx-bootstrap/datepicker/public_api';
import { User } from '../_models/user';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css'],
})
export class RegisterComponent implements OnInit {
  @Output() cancelRegistration = new EventEmitter();
  user: User;
  registerForm: FormGroup;
  bsConfig: Partial<BsDatepickerConfig>;

  constructor(
    private authService: AuthService,
    private router: Router,
    private alertifyService: AlertifyService,
    private fb: FormBuilder
  ) {}

  ngOnInit() {
    this.bsConfig = {
      containerClass: 'theme-red',
    };
    // create the registration form
    this.createRegisterForm();
  }

  createRegisterForm() {
    this.registerForm = this.fb.group(
      {
        gender: ['other'],
        username: ['', [Validators.required, Validators.email]],
        knownAs: ['', Validators.required],
        dateOfBirth: [null, Validators.required],
        city: ['', Validators.required],
        country: ['', Validators.required],
        password: [
          '',
          [
            Validators.required,
            Validators.minLength(8),
            Validators.maxLength(20),
          ],
        ],
        confirmPassword: [
          '',
          [
            Validators.required,
            Validators.minLength(8),
            Validators.maxLength(20),
          ],
        ],
      },
      { validators: this.passwordMatchValidator }
    );
  }

  // check that password and confirm are matches
  passwordMatchValidator(form: FormGroup) {
    return form.get('password').value === form.get('confirmPassword').value
      ? null
      : { mismatch: true };
  }

  register() {
    if (this.registerForm.valid) {
      // clones the form values to an empty object
      this.user = Object.assign({}, this.registerForm.value);
      this.authService.register(this.user).subscribe(() => {
        this.alertifyService.success('you have registered successfully');
      }, error => {
        this.alertifyService.error(error);
      }, () => {
        // log in upon succcessful registration
        this.authService.login(this.user).subscribe(() => {
          this.router.navigate(['/members']);
        })
      })
    }
  }

  cancel() {
    this.cancelRegistration.emit(false);
    this.alertifyService.message('cancelled registration');
  }
}
