{
  description = "Bun Experiments";

  inputs = {
    nixpkgs.url = "github:nixos/nixpkgs?ref=nixos-unstable";
    flake-parts.url = "github:hercules-ci/flake-parts";
  };

  outputs = inputs@{ self, flake-parts, ... }:
    flake-parts.lib.mkFlake { inherit inputs; } {
      systems = [ "aarch64-linux" "x86_64-linux" "aarch64-darwin" ];

      perSystem = { pkgs, system, inputs', ... }: {
        _module.args.pkgs = import inputs.nixpkgs {
          inherit system;
          overlays = [ ];
        };
        packages = { };

        devShells.default = pkgs.mkShell { buildInputs = with pkgs; [ bun ]; };
      };
    };
}
