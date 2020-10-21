import {environment} from "../../environments/environment";

export class HttpRoutes {
  private static backendUrl = environment.backendUrl;
  private static personController = 'api/person';
  private static projectController = 'api/project';

  public static personApiUrl = `${HttpRoutes.backendUrl}${HttpRoutes.personController}`;
  public static projectApiUrl = `${HttpRoutes.backendUrl}${HttpRoutes.projectController}`;
}
