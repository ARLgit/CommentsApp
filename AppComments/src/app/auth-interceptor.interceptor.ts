import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { CookieService } from 'ngx-cookie-service';

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const cookieService = inject(CookieService);
  if (cookieService.check("token"))
  {
    const authorizedRequest = req.clone({
      headers: req.headers.set("Authorization", `Bearer ${cookieService.get("token")}`),
    });
    return next(authorizedRequest);
  }
  return next(req);
};
