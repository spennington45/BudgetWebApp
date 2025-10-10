import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { SourceType } from '../models';
import { Observable } from 'rxjs';
import { environment as env } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class SourceTypeService {

  constructor(private http: HttpClient) {}

  getAllSourceTypes(): Observable<SourceType[]> {
    return this.http.get<SourceType[]>(`${env.BASE_URL}/SourceType/GetAllSourceTypes`);
  }

  getSourceTypeById(id: number): Observable<SourceType> {
    return this.http.get<SourceType>(`${env.BASE_URL}/SourceType/GetSourceTypeById/${id}`);
  }

  addSourceType(sourceType: SourceType): Observable<SourceType> {
    return this.http.post<SourceType>(`${env.BASE_URL}/SourceType/AddSourceType`, sourceType);
  }

  updateSourceType(sourceType: SourceType): Observable<SourceType> {
    return this.http.put<SourceType>(`${env.BASE_URL}/SourceType/UpdateSourceType`, sourceType);
  }

  deleteSourceType(id: number): Observable<void> {
    return this.http.delete<void>(`${env.BASE_URL}/SourceType/DeleteSourceType/${id}`);
  }
}