import {environment} from "../../environments/environment";

export class HttpRoutes {
  private static backendUrl = environment.backendUrl;
  private static personController = 'api/persons';

  public static personApiUrl = `${HttpRoutes.backendUrl}${HttpRoutes.personController}`;
}
