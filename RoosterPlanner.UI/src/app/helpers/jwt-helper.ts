export class JwtHelper {

  public static decodeToken(token: string | null): any {
    if (token) {
      const parts = token.split('.');
      if (parts.length !== 3) {
        throw new Error('JWT must have 3 parts');

      }
      const decoded = this.urlBase64Decode(parts[1]);
      if (!decoded) {
        throw new Error('Cannot decode the token');

      }
      return JSON.parse(decoded);
    } else {
      return null;
    }
  }

  private static urlBase64Decode(str: string): any {
    let output = str.replace(/-/g, '+').replace(/_/g, '/');
    switch (output.length % 4) {
      case 0: { break; }
      case 2: { output += '=='; break; }
      case 3: { output += '='; break; }
      default: {
        throw Error('Illegal base64url string!');
      }
    }
    return decodeURIComponent(window.atob(output));
  }
}
