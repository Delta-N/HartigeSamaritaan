﻿import {environment} from "../../environments/environment";

export class HttpRoutes {
  private static backendUrl = environment.backendUrl;
  private static personController = 'api/persons';
  private static projectController = 'api/projects';
  private static participationController = 'api/participations';
  private static taskController = 'api/tasks';
  private static uploadController = 'api/upload'

  public static personApiUrl = `${HttpRoutes.backendUrl}${HttpRoutes.personController}`;
  public static projectApiUrl = `${HttpRoutes.backendUrl}${HttpRoutes.projectController}`;
  public static participationApiUrl = `${HttpRoutes.backendUrl}${HttpRoutes.participationController}`;
  public static taskApiUrl = `${HttpRoutes.backendUrl}${HttpRoutes.taskController}`;
  public static uploadApiUrl = `${HttpRoutes.backendUrl}${HttpRoutes.uploadController}`;
}
