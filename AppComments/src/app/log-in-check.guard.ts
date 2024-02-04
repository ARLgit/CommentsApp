import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { CookieService } from 'ngx-cookie-service';

export const logInCheckGuard: CanActivateFn = (route, state) => {
  const cookieService = inject(CookieService);
  const router: Router = inject(Router);
  if (cookieService.check("token") && cookieService.check("session"))
  {
    return true;
  }
  else
  {
    router.navigate(["threads"]);
    return false;
  }
};
